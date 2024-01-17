namespace Contracts.User
{
    public class PersonalDataAppendRequest
    {

        public string firstName { get; set; } = null!;
        public string lastName { get; set; } = null!;
        public string patronymic { get; set; } = null!;

        public string phone { get; set; } = null!;
        public string city { get; set; } = null!;
        public string institution { get; set; } = null!;
        public int grade { get; set; } = 1;
        public string? speciality {  get; set; } = null!;

        public int caseId { get; set; }
    }
}
