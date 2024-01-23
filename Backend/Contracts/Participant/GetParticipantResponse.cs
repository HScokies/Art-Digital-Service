using Domain.Enumeration;

namespace Contracts.Participant
{
    public sealed class GetParticipantResponse
    {
        public string firstName { get; set; } = null!;
        public string lastName { get; set; } = null!;
        public string patronymic { get; set; } = null!;
        public int typeId { get; set; }
        public int? caseId { get; set; }
        public string status { get; set; } = Roles.ParticipantsStatus.justRegistered;
        public string phone { get; set; } = null!;
        public string city { get; set; } = null!;
        public string institution { get; set; } = null!;
        public int grade { get; set; } = 1;
        public string? speciality { get; set; } = null!;
        public string? consentFilename { get; set; } = null!;
        public string? solutionFilename { get; set; } = null!;
        public int? rating { get; set; } = null!;
    }
}
