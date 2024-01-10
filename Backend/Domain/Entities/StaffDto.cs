namespace Domain.Entities
{
    public class StaffDto
    {
        public int id {  get; set; }
        public int userId { get; set; }
        public UserDto User { get; set; } = null!;

        public int roleId { get; set; }
        public StaffRoleDto Role { get; set; } = null!;
    }
}
