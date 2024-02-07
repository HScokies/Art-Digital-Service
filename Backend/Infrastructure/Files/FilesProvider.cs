using Contracts.File;
using Contracts.Participant;
using Domain.Core.Primitives;
using Domain.Enumeration;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using MimeDetective;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Diagnostics;
using System.Text.Json;

namespace Infrastructure.Files
{
    public class FilesProvider : IFilesProvider
    {
        private readonly string userFiles = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Files", "user-uploaded");
        private readonly string legalFiles = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Files", "legal");
        private readonly string certificateFiles = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Files", "certificate");
        private readonly ContentInspector contentInspector;
        public FilesProvider()
        {
            contentInspector = new ContentInspectorBuilder()
            {
                Definitions = MimeDetective.Definitions.Default.All()
            }.Build();

            Directory.CreateDirectory(userFiles);
            Directory.CreateDirectory(legalFiles);
            Directory.CreateDirectory(certificateFiles);
        }
        private async Task<Result<MemoryStream>> DownloadFileAsync(string directory, string fileName, CancellationToken cancellationToken)
        {
            try
            {
                string path = Path.Combine(directory, fileName);
                using FileStream fs = new(path, FileMode.Open);

                MemoryStream stream = new();                
                await fs.CopyToAsync(stream, cancellationToken);
                stream.Position = 0;

                return new Result<MemoryStream>(stream);
            }
            catch (FileNotFoundException)
            {
                return new Result<MemoryStream>(CommonErrors.File.NotFound);
            }
            catch(Exception)
            {
                return new Result<MemoryStream>(CommonErrors.Unknown);
            }
        }

        private string getExtension(IFormFile file)
        {
            using var stream = file.OpenReadStream();
            var InspectResults = contentInspector.Inspect(stream).ByFileExtension();
            return InspectResults.FirstOrDefault()?.Extension!;
            
        }

        public Result<string> getMimeType(IFormFile file)
        {
            using var stream = file.OpenReadStream();
            var InspectResults = contentInspector.Inspect(stream).ByMimeType();
            var MIMEtype = InspectResults.FirstOrDefault()?.MimeType;
            return MIMEtype is null ? new Result<string>(CommonErrors.File.UnknownMimeType) : new Result<string>(MIMEtype);
        }

        public Result<string> getMimeType(Stream stream)
        {
            var InspectResults = contentInspector.Inspect(stream).ByMimeType();
            var MIMEtype = InspectResults.FirstOrDefault()?.MimeType;
            stream.Position = 0;
            return MIMEtype is null ? new Result<string>(CommonErrors.File.UnknownMimeType) : new Result<string>(MIMEtype);
        }

        private async Task<Result<string>> UploadFileAsync(string directory, IFormFile file, CancellationToken cancellationToken)
        {
            try
            {
                string safeFilename = $"{Guid.NewGuid()}.{getExtension(file)}";
                string path = Path.Combine(directory, safeFilename);                
                await using (FileStream fs = new(path, FileMode.Create))
                {
                    await file.CopyToAsync(fs, cancellationToken);
                }
                return new Result<string>(safeFilename);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.Message);
                return new Result<string>(CommonErrors.Unknown);
            }            
        }

        private async Task<Result<string>> UploadFileAsync(string directory, IFormFile file, string fileName, CancellationToken cancellationToken)
        {
            try
            {
                string path = Path.Combine(directory, fileName);
                await using (FileStream fs = new(path, FileMode.Create))
                {
                    await file.CopyToAsync(fs, cancellationToken);
                }
                return new Result<string>(path);
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.Message);
                return new Result<string>(CommonErrors.Unknown);
            }
        }

        private Result<bool> DeleteFile(string directory, string fileName)
        {
            try
            {
                string path = Path.Combine(directory, fileName);
                File.Delete(path);
                return new Result<bool>(true);
            }
            catch(Exception ex)
            {
                Debug.Fail(ex.Message);
                return new Result<bool>(CommonErrors.Unknown);
            }
        }

        public async Task<Result<string>> UploadUserFileAsync(IFormFile file, CancellationToken cancellationToken) => await UploadFileAsync(userFiles, file, cancellationToken);

        public void DropUserFile(string fileName) => DeleteFile(userFiles, fileName);

        public async Task<Result<FileResponse>> DownloadUserFileAsync(string fileName, CancellationToken cancellationToken)
        {
            var streamResult = await DownloadFileAsync(userFiles, fileName, cancellationToken);
            if (!streamResult.isSuccess)
                return new Result<FileResponse>(CommonErrors.Unknown);

            var mimeTypeResult = getMimeType(streamResult.value);
            if (!mimeTypeResult.isSuccess)
                return new Result<FileResponse>(CommonErrors.Unknown);
                        
            var fileData = new FileResponse()
            {
                fileStream = streamResult.value,
                contentType = mimeTypeResult.value,
            };

            return new Result<FileResponse>(fileData);
        }
    
        public async Task<Result<FileResponse>> DownloadLegalFileAsync(string fileName, CancellationToken cancellationToken)
        {
            var streamResult = await DownloadFileAsync(legalFiles, fileName, cancellationToken);
            if (!streamResult.isSuccess)
                return new Result<FileResponse>(streamResult.error);

            var fileData = new FileResponse()
            {
                fileStream = streamResult.value,
                contentType = "application/pdf",
            };

            return new Result<FileResponse>(fileData);
        }

        public async Task<Result<string>> UploadLegalFileAsync(IFormFile file, string fileName, CancellationToken cancellationToken) => await UploadFileAsync(legalFiles, file, fileName, cancellationToken);

        public async Task<Result<CertificateModel>> GetCertificateConfigAsync(CancellationToken cancellationToken)
        {
            string path = Path.Combine(certificateFiles, "certificatecfg.json");
            try
            {
                using (FileStream fs = new FileStream(path, FileMode.Open))
                {
                    CertificateModel? config = await JsonSerializer.DeserializeAsync<CertificateModel>(fs, cancellationToken: cancellationToken);
                    return config is null ? new Result<CertificateModel>(CommonErrors.Unknown) : new Result<CertificateModel>(config);
                }
            }
            catch (FileNotFoundException)
            {
                return new Result<CertificateModel>(CommonErrors.File.NotFound);
            }
            catch(Exception ex)
            {
                Debug.Fail(ex.Message);
                return new Result<CertificateModel>(CommonErrors.Unknown);
            }
            
        }

        public async Task<Result<FileResponse>> DownloadCertificateBlankAsync(CancellationToken cancellationToken)
        {

            var streamResult = await DownloadFileAsync(certificateFiles, "blank", cancellationToken);
            if (!streamResult.isSuccess)
                return new Result<FileResponse>(streamResult.error);

            var mimeResult = getMimeType(streamResult.value);
            if (!mimeResult.isSuccess)
                return new Result<FileResponse>(mimeResult.error);
            
            var fileData = new FileResponse()
            {
                fileStream = streamResult.value,
                contentType = mimeResult.value,
                fileName = "blank"
            };

            return new Result<FileResponse>(fileData);
        }

        public async Task<Result<string>> UploadCertificateBlankAsync(IFormFile file, CancellationToken cancellationToken) => await UploadFileAsync(certificateFiles, file, "blank", cancellationToken);

        public async Task<Result<bool>> UploadCertificateConfigAsync(AppendCertificateRequest request, CancellationToken cancellationToken)
        {
            string path = Path.Combine(certificateFiles, "certificatecfg.json");
            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                CertificateModel config = new()
                {
                    paddingLeft = request.paddingLeft,
                    paddingTop = request.paddingTop,
                    paddingRight = request.paddingRight,
                    paddingBottom = request.paddingBottom,
                };
                await JsonSerializer.SerializeAsync<CertificateModel>(fs, config, cancellationToken: cancellationToken);
            }
            return new Result<bool>();                
        }

        public FileResponse DownloadCertificate(GetCertificateRequest request)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            string blankFilePath = Path.Combine(certificateFiles, "blank");
            
            MemoryStream memoryStream = new MemoryStream();
            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Background().Image(blankFilePath).FitArea();
                    page.Size(PageSizes.A4.Landscape());
                    page.Content().DefaultTextStyle(style => style.FontFamily(Fonts.Calibri).FontSize(20)).PaddingLeft(50).PaddingTop(45).PaddingRight(200).Text(t =>
                    {
                        t.Line("Сертификат").FontSize(50).ExtraBlack().LineHeight(1);
                        t.Line("подтверждает, что");
                        t.Line(request.participantName).FontSize(40).Black();
                        t.Line("Принял(а) участие в олимпиаде");
                        t.Line("предпрофессиональных навыков «Art.Digital.Service»");
                        t.Span("по направлению");
                        t.Span($"«{request.caseName}»");
                    });
                });
            }).GeneratePdf(memoryStream);
            memoryStream.Position = 0;

            return new FileResponse()
            {
                fileStream = memoryStream,
                contentType = "application/pdf",
                fileName = "Сертификат участника олимпиады"
            };
        }
    }
}
