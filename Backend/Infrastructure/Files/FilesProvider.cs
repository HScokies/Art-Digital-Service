using Contracts.File;
using Domain.Core.Primitives;
using Domain.Enumeration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MimeDetective;
using System.Diagnostics;
using System.IO;

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
    }
}
