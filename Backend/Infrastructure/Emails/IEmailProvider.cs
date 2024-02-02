using MimeKit;
using System.Net.Mail;

namespace Infrastructure.Emails
{
    public interface IEmailProvider
    {
        public Task SendWelcomeEmail(MailboxAddress recipient, string youtubeId, CancellationToken cancellationToken);
        public Task SendPasswordResetEmail(MailboxAddress recipient, string resetToken, CancellationToken cancellationToken);
    }
}
