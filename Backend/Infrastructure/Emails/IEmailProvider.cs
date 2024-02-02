using MimeKit;
using System.Net.Mail;

namespace Infrastructure.Emails
{
    public interface IEmailProvider
    {
        Task SendWelcomeEmail(MailboxAddress recipient, string youtubeId, bool isAdult, CancellationToken cancellationToken);
        Task SendPasswordResetEmail(MailboxAddress recipient, string resetToken, CancellationToken cancellationToken);
    }
}
