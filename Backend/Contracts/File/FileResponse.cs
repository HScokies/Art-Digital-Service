namespace Contracts.File
{
    public class FileResponse
    {
        public MemoryStream fileStream { get; set; } = null!;
        public string contentType { get; set; } = null!;
        public string? fileName { get; set; } = null!;
    }
}
