namespace Infrastructure.Emails
{
    public interface IEmailProvider
    {
        public Task SendWelcomeEmail(string recipientEmail, string videoURL);
        public Task SendPasswordResetEmail(string recipientEmail, string recipientName, string resetUrl);
    }
}
