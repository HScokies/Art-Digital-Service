using Domain.Enumeration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class StaffRole
    {
        public string displayedName { get; set; } = null!;
        public List<AccessLevels> permissions { get; set; } = null!;
    }
}
