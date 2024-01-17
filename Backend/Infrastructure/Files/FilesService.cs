using Microsoft.AspNetCore.Http;

namespace Infrastructure.Files
{
    public class FilesService : IFilesService
    {
        private readonly string userFiles = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Files", "user-uploaded");
        public string DownloadFile(string id, string? displayedName)
        {
            throw new NotImplementedException();
        }

        public async Task UploadFile(IFormFile file, string safeFilename)
        {
            var path = Path.Combine(userFiles, safeFilename);
            await using (FileStream fs = new(path, FileMode.Create))
            {
                await file.CopyToAsync(fs);
            }
        }
    }
}
