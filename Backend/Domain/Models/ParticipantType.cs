using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class ParticipantType
    {
        public string name { get; set; } = null!;
        public bool isAdult { get; set; } = false; // Используется для определения полей на форме с персональными данными
    }
}
