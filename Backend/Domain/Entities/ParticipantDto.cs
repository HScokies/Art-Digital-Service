using Domain.Enumeration;
using System.Text.Json.Serialization;

namespace Domain.Entities
{
    public class ParticipantDto
    {
        public int id {  get; set; }
        public int userId { get; set; }
        [JsonIgnore]
        public UserDto User { get; set; } = null!;

        public int typeId { get; set; }
        [JsonIgnore]
        public ParticipantTypeDto Type { get; set; } = null!;

        public int? caseId { get; set; }
        [JsonIgnore]
        public CaseDto Case { get; set; } = null!;

        public string status { get; set; } = Roles.ParticipantsStatus.justRegistered;

        public string phone { get; set; } = string.Empty;
        public string city { get; set; } = string.Empty;
        public string institution { get; set; } = string.Empty;
        public int grade { get; set; } = 1;
        public string? speciality { get; set; } = string.Empty;

        public string? consentFilename { get; set; } = null!;
        public string? solutionFilename { get; set; } = null!;

        public int? rating { get; set; } = null!;
    }
}
