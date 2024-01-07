using Domain.Enumeration;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class StaffDto
    {
        public int id {  get; set; }
        public int userId { get; set; }
        public UserDto user { get; set; } = null!;

        public int roleId { get; set; }
        public StaffRoleDto role { get; set; } = null!; 
    }
}
