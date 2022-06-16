using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;

namespace Application.UseCases.Email.Commands.SendEmail
{
    public class SendEmailCommandHandler : IRequestHandler<SendEmailCommand>
    {
        private readonly IEmailSender _emailSender;

        public SendEmailCommandHandler(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        public async Task<Unit> Handle(SendEmailCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Subject))
            {
                request.Subject = "Здравствуйте!";
            }

            if (string.IsNullOrWhiteSpace(request.Content))
            {
                request.Content = $"<div>" +
                                  $"<h1>{request.Subject}</h1>" +
                                  $"<hr />" +
                                  $"<p>Дорогой пользователь, спасибо за то, что оставили нам свои данные!</p>" +
                                  $"</div>";
            }

            await _emailSender.SendEmailAsync(new Message(new[] {request.EmailAddress}, request.Subject, request.Content), cancellationToken);

            return Unit.Value;
        }
    }
}