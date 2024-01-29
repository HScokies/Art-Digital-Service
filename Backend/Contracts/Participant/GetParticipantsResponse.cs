namespace Contracts.User
{
    public sealed class GetParticipantsResponse
    {
        public ParticipantPreview[] rows { get; set; } = null!;
        public int currentPage { get; set; }
        public int pagesTotal { get; set; }
    }

    public sealed class ParticipantPreview
    {
        public int id { get; set; }
        public string fullName { get; set; } = null!;
        public string typeName { get; set; } = null!;
        public string caseName { get; set; } = null!;
        public int? rating { get; set; }
    }
}
