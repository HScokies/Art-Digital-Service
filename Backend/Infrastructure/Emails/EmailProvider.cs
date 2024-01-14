using MimeKit;
using MimeKit.Text;
using MailKit.Security;
using MailKit.Net.Smtp;

namespace Infrastructure.Emails
{
    public class EmailProvider : IEmailProvider
    {
        private readonly string templatesPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Emails","Templates");
        private readonly MailboxAddress sender;
        private readonly string login;
        private readonly string password;
        private readonly string host;
        private readonly int port;

        public EmailProvider(string login, string password, string host, int port)
        {
            this.login = login;
            this.password = password;
            this.host = host;
            this.port = port;
            this.sender = new MailboxAddress("Цифра. Дизайн. Сервис", "noreply@midis.ru");
        }

        public async Task SendPasswordResetEmail(string recipientEmail, string recipientName, string resetUrl)
        {
            var path = Path.Combine(templatesPath, "password-reset.html");
            string contents = File.ReadAllText(path);
            MimeMessage message = new()
            {
                From =
                {
                    sender
                },
                To =
                {
                    new MailboxAddress(recipientName, recipientEmail)
                },
                Subject = "Запрос на сброс пароля",
                Body =  new TextPart(TextFormat.Html)
                {
                    Text = contents
                }
            };

            await SendEmailAsync(message);
        }

        public Task SendWelcomeEmail(string recipientEmail, string videoURL)
        {
            throw new NotImplementedException();
        }

        private async Task SendEmailAsync(MimeMessage email, CancellationToken cancellationToken = default)
        {
            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(host, port, SecureSocketOptions.StartTls, cancellationToken);
            await smtp.AuthenticateAsync(login, password, cancellationToken);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}
