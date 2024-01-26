namespace Contracts.Cases
{
    public class UpsertCaseRequest
    {
        public string name { get; set; } = null!;
        public string task { get; set; } = null!;
        public string youtubeId { get; set; } = null!;
        public string[] stage { get; set; } = null!;
        public string[] criteria { get; set; } = null!;

    }
}
