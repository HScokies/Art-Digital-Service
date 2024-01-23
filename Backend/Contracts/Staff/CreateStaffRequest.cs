namespace Contracts.Staff
{
    public class CreateStaffRequest
    {
        public string email { get; set; } = null!;
        public string password { get; set; } = null!;
        public string firstName { get; set; } = null!;
        public string lastName { get; set; } = null!;
        public string patronymic { get; set; } = null!;
        public int roleId { get; set; }
    }
}
