using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enumeration
{
    /// <summary>
    /// Перечисление, используемое для быстрого определения принадлежности пользователя к сотрудникам и вывода соответствующих полей на форме с персональными данными
    /// </summary>
    public enum UserTypes
    {
        Child,
        Adult,
        Staff
    }
}
