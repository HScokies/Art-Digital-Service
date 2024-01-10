using Domain.Enumeration;

namespace Domain.Entities
{
    public class ParticipantDto
    {
        public int id {  get; set; }
        public int userId { get; set; }
        public UserDto User { get; set; } = null!;

        public int typeId { get; set; }
        public ParticipantTypeDto Type { get; set; } = null!;

        public int? caseId { get; set; }
        public CaseDto Case { get; set; } = null!;

        public participant_status status { get; set; }

        public string phone { get; set; } = string.Empty;
        public string city { get; set; } = string.Empty;
        public string institution { get; set; } = string.Empty;
        public int? grade { get; set; }
        public string? speciality { get; set; } = string.Empty;

        public string? consentFilename { get; set; } = null!;
        public string? solutionFilename { get; set; } = null!;

        public int? rating { get; set; } = null!;
    }
}
