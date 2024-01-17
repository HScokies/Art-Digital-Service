﻿using MimeKit;
using System.Net.Mail;

namespace Infrastructure.Emails
{
    public interface IEmailProvider
    {
        public Task SendWelcomeEmail(MailboxAddress recipient, string youtubeId, string continueUrl, CancellationToken cancellationToken);
        public Task SendPasswordResetEmail(MailboxAddress recipient, string resetUrl, CancellationToken cancellationToken);
    }
}
