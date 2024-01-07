using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enumeration
{
    public enum AccessLevels
    {
        readUsers,
        createUsers,
        rateUsers, //aka limited update
        updateUsers,
        deleteUsers,

        readCases,
        createCases,
        updateCases,
        deleteCases,

        readStaff,
        createStaff,
        updateStaff,
        deleteStaff
    }
}
