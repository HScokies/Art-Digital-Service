namespace Contracts.User
{
    public class LoginRequest
    {
        public string email { get; set; } = null!;
        public string password { get; set; } = null!;
    }
}
