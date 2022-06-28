using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.UseCases.Email.Commands.SendEmail
{
    public class SendEmailCommand : IRequest
    {
        public string EmailAddress { get; set; }

        public string Subject { get; set; }

        public string Content { get; set; }

        public byte[] Attachment { get; set; }
    }
}