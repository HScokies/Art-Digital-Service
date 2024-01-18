using System.Text.RegularExpressions;
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
            Span<string> validMimeTypes = ["image/bmp", "image/jpeg", "image/png", "application/pdf"];
            return validMimeTypes.Contains(MimeType);
        }
        public static bool isValidSolutionMimeType(string MimeType)
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
