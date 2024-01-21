namespace Contracts.Participant
{
    public class ParticipantExportModel
    {
        public string firstName { get; set; } = null!;
        public string lastName { get; set; } = null!;
        public string patronymic { get; set; } = null!;
        public string userType { get; set; } = null!;
        public string caseName { get; set; } = null!;
        public int? rating { get; set; } = null!;
        public string city { get; set; } = null!;        
        public string phone { get; set; } = null!;
        public string email { get; set; } = null!;
        public string institution { get; set; } = null!;
        public string speciality { get; set; } = null!;
        public string grade { get; set; } = null!;
    }
}
