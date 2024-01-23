using Domain.Enumeration;
using System.Text.Json.Serialization;

namespace Domain.Entities
{
    public class StaffRoleDto
    {
        public int id {  get; set; }
        [JsonIgnore]
        public ICollection<StaffDto> Staff { get; set; } = null!;
        public string name { get; set; } = null!;
        [JsonIgnore]
        public List<string> PermissionsList { get; set; } = new();
    }
}
