using Microsoft.AspNetCore.Http;

namespace Contracts.User
{
    public sealed class UpdateParticipantRequest
    {
        public int? userTypeId { get; set; } = null!;
        public int? caseId { get; set; } = null!;

        public string email { get; set; } = null!;
        public string phone { get; set; } = null!;
        public string firstName { get; set; } = null!;
        public string lastName { get; set; } = null!;
        public string patronymic { get; set; } = null!;
        public string city { get; set; } = null!;
        public string institution { get; set; } = null!;
        public int grade { get; set; } = 1;
        public string? speciality { get; set; } = null;  
        public IFormFile? consent { get; set; } = null!;
        public IFormFile? solution { get; set; } = null!;
        public int score { get; set; } = 1;
        public string status { get; set; } = null!;
    }
}
