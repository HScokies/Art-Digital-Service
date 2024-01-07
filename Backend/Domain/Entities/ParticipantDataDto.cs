
namespace Domain.Entities
{
    public class ParticipantDataDto
    {
        public int id { get; set; }
        
        public int userId { get; set; }
        public UserDto user { get; set; } = null!;       
                
        public int caseId { get; set; }
        public CaseDto Case { get; set; } = null!;

        public string phone { get; set; } = null!;
        public string city { get; set; } = null!;
        public string institution { get; set; } = null!;
        public int grade { get; set; }
        public string? speciality { get; set; } = null!;

        public string? consentFilename { get; set; } = null!;
        public string? solutionFilename { get; set; } = null!;

        public int? rating { get; set; } = null!;
    }
}
