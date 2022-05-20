using MediatR;

namespace Application.UseCases.Payment.Commands.DeletePayment
{
    public class DeletePaymentCommand : IRequest
    {
        public int? Id { get; set; }
    }
}