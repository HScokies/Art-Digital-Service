namespace Contracts.Staff
{
    public sealed class StaffExportModel
    {
        public string firstName { get; set; } = null!;
        public string lastName { get; set; } = null!;
        public string patronymic { get; set; } = null!;
        public string role { get; set; } = null!;
        public string email { get; set; } = null!;
    }
}
