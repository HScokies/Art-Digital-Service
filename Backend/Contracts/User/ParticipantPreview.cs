namespace Contracts.User
{
    public class ParticipantPreview
    {
        public int participantId { get; set; }
        public string fullName { get; set; } = null!;
        public string typeName { get; set; } = null!;
        public string caseName { get; set; } = null!;
        public int? score = null!;
    }

    public class GetParticipantResponse
    {
        public ParticipantPreview[] participants { get; set; } = null!;
        public int currentPage { get; set; }
        public int pagesTotal { get; set; }
    }
}
