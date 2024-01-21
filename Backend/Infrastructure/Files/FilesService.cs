﻿using Domain.Core.Primitives;
using Domain.Enumeration;
using Microsoft.AspNetCore.Http;
using MimeDetective;
using System.Diagnostics;
using System.IO;

namespace Infrastructure.Files
{
    public class FilesService : IFilesService
    {
        private readonly string userFiles = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Files", "user-uploaded");
        private readonly ContentInspector contentInspector;
        public FilesService()
        {
            contentInspector = new ContentInspectorBuilder()
            {
                Definitions = MimeDetective.Definitions.Default.All()
            }.Build();

            Directory.CreateDirectory(userFiles);
        }
        public async Task<Result<MemoryStream>> DownloadFileAsync(string id, string? displayedName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
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
    }
}
