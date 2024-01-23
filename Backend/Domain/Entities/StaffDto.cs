using System.Text.Json.Serialization;

namespace Domain.Entities
{
    public class StaffDto
    {
        public int id {  get; set; }
        public int userId { get; set; }
        [JsonIgnore]
        public UserDto User { get; set; } = null!;

        public int roleId { get; set; }
        [JsonIgnore]
        public StaffRoleDto Role { get; set; } = null!;
    }
}
