using Domain.Enumeration;

namespace Domain.Entities
{
    public class StaffRoleDto
    {
        public int id {  get; set; }
        public ICollection<StaffDto> Staff { get; set; } = null!;
        public string name { get; set; } = null!;
        public List<string> PermissionsList { get; set; } = new();
    }
}
