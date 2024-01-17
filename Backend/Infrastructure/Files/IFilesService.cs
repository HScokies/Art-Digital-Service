using Microsoft.AspNetCore.Http;

namespace Infrastructure.Files
{
    public interface IFilesService
    {
        public Task UploadFile(IFormFile file, string id);
        public string DownloadFile(string id, string? displayedName);
    }
}
