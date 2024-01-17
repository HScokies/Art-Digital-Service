using MimeKit;
using MimeKit.Text;
using MailKit.Security;
using MailKit.Net.Smtp;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;

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
        private readonly string APP_URL;

        public EmailProvider(string login, string password, string host, int port, IHttpContextAccessor httpContextAccessor)
        {
            this.login = login;
            this.password = password;
            this.host = host;
            this.port = port;
            this.sender = new MailboxAddress("Цифра. Дизайн. Сервис", "noreply@midis.ru");

            var RequestContext = httpContextAccessor.HttpContext?.Request;
            APP_URL = RequestContext is null? "https://localhost:7220" : $"{RequestContext.Scheme}://{RequestContext.Host}";
        }

        public async Task SendPasswordResetEmail(MailboxAddress recipient, string resetUrl, CancellationToken cancellationToken)
        {
            const string templateName = "password-reset";
            const string subject = "Инструкции для смены пароля учетной записи олимпиады 'Цифра. Дизайн. Сервис'";
            Dictionary<string, string> placeholders = new()
            {
                {"{{user}}", recipient.Name},
                {"{{url}}", resetUrl }
            };
            
            var content = await GetTemplate(templateName, cancellationToken);
            content = FillPlaceholders(content, placeholders);

            var message = CreateMessage(recipient, subject, content);

            await SendEmailAsync(message);
        }

        public async Task SendWelcomeEmail(MailboxAddress recipient, string youtubeId, string continueUrl, CancellationToken cancellationToken)
        {
            const string templateName = "welcome-message";
            const string subject = "Добро пожаловать на олимпиаду 'Цифра. Дизайн. Сервис'";
            Dictionary<string, string> placeholders = new()
            {
                {"{{base_url}}", APP_URL},
                {"{{user}}", recipient.Name},
                {"{{youtubeId}}", youtubeId },
                {"{{url}}", continueUrl }
            };

            var content = await GetTemplate(templateName, cancellationToken);
            content = FillPlaceholders(content, placeholders);

            var message = CreateMessage(recipient, subject, content);

            await SendEmailAsync(message);
        }
        private async Task<string> GetTemplate(string name, CancellationToken cancellationToken)
        {
            var path = Path.Combine(templatesPath, $"{name}.html");
            using StreamReader reader = new(path);
            return await reader.ReadToEndAsync(cancellationToken);
        }
        private string FillPlaceholders(string content, Dictionary<string, string> placeholders)
        {
            foreach (var placeholder in placeholders)
            {
                if (content.Contains(placeholder.Key))
                {
                    content = Regex.Replace(content, placeholder.Key, placeholder.Value);
                }
            }
            return content;
        }
        private MimeMessage CreateMessage(MailboxAddress recipient, string subject, string htmlContent)
        {
            MimeMessage message = new()
            {
                From = { sender },
                To = { recipient },
                Subject = subject,
                Body = new TextPart(TextFormat.Html)
                {
                    Text = htmlContent
                }
            };
            return message;
        }
        private async Task SendEmailAsync(MimeMessage email, CancellationToken cancellationToken = default)
        {
            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(host, port, SecureSocketOptions.StartTls, cancellationToken);
            await smtp.AuthenticateAsync(login, password, cancellationToken);
            await smtp.SendAsync(email, cancellationToken);
            await smtp.DisconnectAsync(true, cancellationToken);
        }
    }
}
