using Domain.Entities;
using MediatR;

namespace Application.UseCases.Payment.Commands.CreatePayment
{
    public class CreatePaymentCommand : IRequest<int>
    {
        public PaymentType Type { get; set; }

        public int OrderId { get; set; }

        public string CreditCardNumber { get; set; }

        public int CardExpMonth { get; set; }

        public int CardExpYear { get; set; }
    }
}