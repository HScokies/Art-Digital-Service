using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;

namespace Contracts.Participant
{
    public sealed class CreateParticipantRequest
    {
        public int typeId { get; set; } = 1;
        public int caseId { get; set; } = 1;

        public string email { get; set; } = null!;
        public string password { get; set; } = null!;
        public string phone { get; set; } = null!;
        public string city { get; set; } = null!;
        public string firstName { get; set; } = null!;
        public string lastName { get; set; } = null!;
        public string patronymic { get; set; } = null!;
        public string institution { get; set; } = null!;
        public int grade { get; set; } = 1;
        public string? speciality { get; set; } = null;
        public IFormFile? consent { get; set; } = null!;
        public IFormFile? solution { get; set; } = null!;

        [JsonIgnore]
        public string? consentFilename { get; set; } = null!;
        [JsonIgnore]
        public string? solutionFilename { get; set; } = null!;
    }
}
