using Domain.Enumeration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class StaffRoleDto
    {
        public int id {  get; set; }
        public string name { get; set; } = null!;
        public List<AccessLevels> permissionsList { get; set; } = null!;
    }
}
