using System.Text.RegularExpressions;
using Domain.Enumeration;
using Microsoft.IdentityModel.Tokens;

namespace Domain.Core.Utility
{
    public static class Ensure
    {
        public static bool isEmail(string email)
        {
            return Regex.IsMatch(email, @"^(([^<>()[\]\\.,;:\s@""]+(\.[^<>()[\]\\.,;:\s@""]+)*)|.("".+""))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$");
        }

        public static bool isPassword(string password)
        {
            if (password.Length < 7 || password.Length > 24)
                return false;
            if (password.IndexOf(" ") != -1)
                return false;
            if (!Regex.IsMatch(password, @"\d") || !Regex.IsMatch(password, @"[a-zA-Z]"))
                return false;
            return true;
        }
        
        public static bool isPhone(string phone)
        {
            if (!phone.StartsWith("+7 "))
                return false;
            if (phone.Length != 13)
                return false;
            for (int i=3; i < phone.Length; i++)
            {
                if (!Char.IsDigit(phone[i]))
                    return false;
            }
            return true;
        }

        public static bool isStatus(string status) => status switch
        {
            Roles.ParticipantsStatus.justRegistered => true,
            Roles.ParticipantsStatus.sentPersonalData => true,
            Roles.ParticipantsStatus.awaitingResults => true,
            Roles.ParticipantsStatus.droppedOut => true,
            Roles.ParticipantsStatus.invited => true,
            _ => false
        };

        public static bool isValidExpireTime(DateTime? notBefore, DateTime? expires, SecurityToken securityToken, TokenValidationParameters validationParameters)
        {
            if (expires != null)
            {
                return DateTime.UtcNow < expires;
            }
            return false;
        }

        public static bool isValidConsentMimeType(string MimeType)
        {
            HashSet<string> validMimeTypes = ["image/bmp", "image/jpeg", "image/png", "application/pdf"];
            return validMimeTypes.Contains(MimeType);
        }
        public static bool isValidSolutionMimeType(string MimeType)
        {
            return MimeType == "application/pdf";
        }

        public static bool isValidLegalMimeType(string MimeType)
        {
            return MimeType == "application/pdf";
        }

        public static bool isValidFileSize(long size)
        {
            const int MegaByte = 1_048_576;
            return size < (MegaByte * 3);
        }
    }
}
