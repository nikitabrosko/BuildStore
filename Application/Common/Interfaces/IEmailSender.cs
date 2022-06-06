using System.Threading;
using System.Threading.Tasks;
using Application.Common.Models;

namespace Application.Common.Interfaces
{
    public interface IEmailSender
    {
        void SendEmail(Message message);
        Task SendEmailAsync(Message message, CancellationToken cancellationToken);
    }
}