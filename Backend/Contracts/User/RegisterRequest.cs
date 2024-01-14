using System.ComponentModel.DataAnnotations;

namespace Contracts.User
{
    public sealed class RegisterRequest
    {
        public string email { get; set; } = null!;
        public  string password { get; set;} = null!;
        public int userType { get; set; }
    }
}
