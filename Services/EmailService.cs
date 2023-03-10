using OttBlog.ViewModels;
using OttBlog.Services.Interfaces;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using System.Threading.Tasks;
//using Azure.Core;

namespace OttBlog.Services
{
    public class EmailService : IBlogEmailSender
    {
        private readonly MailSettings _mailSettings;

        //public EmailService(MailSettings mailSettings)
        //{
        //    _mailSettings = mailSettings;
        //}


        public EmailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        public async Task SendContactEmailAsync(string email, string name, string subject, string message)
        {
            var newEmail = new MimeMessage();
            newEmail.Sender = MailboxAddress.Parse(_mailSettings.Mail);
            newEmail.To.Add(MailboxAddress.Parse(_mailSettings.Mail));
            newEmail.Subject = subject;

            var builder = new BodyBuilder();
            builder.HtmlBody = $"<b>{name}</b> has sent you an newEmail and can be reached at: <b>{_mailSettings.Mail}</b><br/><br/>{message}";

            newEmail.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);

            await smtp.SendAsync(newEmail);

            smtp.Disconnect(true);
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var newEmail = new MimeMessage();
            newEmail.Sender = MailboxAddress.Parse(_mailSettings.Mail);
            newEmail.To.Add(MailboxAddress.Parse(email));
            newEmail.Subject = subject;

            var builder = new BodyBuilder()
            {
                HtmlBody = htmlMessage
            };

            newEmail.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);

            await smtp.SendAsync(newEmail);
            smtp.Disconnect(true);
        }
    }
}
