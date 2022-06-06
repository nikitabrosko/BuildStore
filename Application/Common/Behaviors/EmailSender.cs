using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Models;
using MailKit.Net.Smtp;
using MimeKit;

namespace Application.Common.Behaviors
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailConfiguration _emailConfiguration;

        public EmailSender(EmailConfiguration emailConfiguration)
        {
            _emailConfiguration = emailConfiguration;
        }

        public void SendEmail(Message message)
        {
            var emailMessage = CreateEmailMessage(message);

            Send(emailMessage);
        }

        public async Task SendEmailAsync(Message message, CancellationToken cancellationToken)
        {
            var emailMessage = CreateEmailMessage(message);

            await SendAsync(emailMessage, cancellationToken);
        }

        private MimeMessage CreateEmailMessage(Message message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_emailConfiguration.Username, _emailConfiguration.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) {Text = message.Content};

            return emailMessage;
        }

        private void Send(MimeMessage mailMessage)
        {
            using var client = new SmtpClient();

            try
            {
                client.Connect(_emailConfiguration.SmtpServer, _emailConfiguration.Port, true);
                client.Authenticate(_emailConfiguration.Username, _emailConfiguration.Password);

                client.Send(mailMessage);
            }
            finally
            {
                client.Disconnect(true);
                client.Dispose();
            }
        }

        private async Task SendAsync(MimeMessage mailMessage, CancellationToken cancellationToken)
        {
            using var client = new SmtpClient();

            try
            {
                await client.ConnectAsync(_emailConfiguration.SmtpServer, _emailConfiguration.Port, true, cancellationToken);
                await client.AuthenticateAsync(_emailConfiguration.Username, _emailConfiguration.Password, cancellationToken);

                await client.SendAsync(mailMessage, cancellationToken);
            }
            finally
            {
                await client.DisconnectAsync(true, cancellationToken);
                client.Dispose();
            }
        }
    }
}