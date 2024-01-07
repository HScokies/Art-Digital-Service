using Domain.Core.Primitives;
using Domain.Enumeration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Domain.Core.Utility
{
    public static class Ensure
    {
        public static bool isStaff(UserTypes userType)
        {
            return userType.Equals(UserTypes.Staff);
        }

        public static bool isEmail(string email)
        {
            return Regex.IsMatch(email, @"^(([^<>()[\]\\.,;:\s@""]+(\.[^<>()[\]\\.,;:\s@""]+)*)|.("".+""))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$");
        }
    }
}
