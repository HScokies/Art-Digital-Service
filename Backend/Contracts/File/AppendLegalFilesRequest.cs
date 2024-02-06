using Microsoft.AspNetCore.Http;

namespace Contracts.File
{
    public class AppendLegalFilesRequest
    {
        public IFormFile? regulations { get; set; } = null!;
        public IFormFile? privacyPolicy { get; set; } = null!;
        public IFormFile? adultConsent { get; set; } = null!;
        public IFormFile? youthConsent { get; set; } = null!;
    }
}
