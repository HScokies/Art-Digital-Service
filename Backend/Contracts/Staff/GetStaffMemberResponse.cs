namespace Contracts.Staff
{
    public sealed class GetStaffMemberResponse
    {
        public string firstName { get; set; } = null!;
        public string lastName { get; set; } = null!;
        public string patronymic { get; set; } = null!;
        public string email { get; set; } = null!;
        public int roleId { get; set; }
    }
}
