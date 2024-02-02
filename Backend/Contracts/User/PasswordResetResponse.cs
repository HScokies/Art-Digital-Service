namespace Contracts.User
{
    public class PasswordResetResponse
    {
        public string name { get; set; } = null!;
        public string email { get; set; } = null!;
        public string token { get; set; } = null!;
    }
}
