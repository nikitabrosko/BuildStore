using Domain.Entities;
using MediatR;

namespace Application.UseCases.Payment.Commands.UpdatePayment
{
    public class UpdatePaymentCommand : IRequest
    {
        public int? Id { get; set; }

        public bool Allowed { get; set; }

        public PaymentType Type { get; set; }

        public string CreditCardNumber { get; set; }

        public int CardExpMonth { get; set; }

        public int CardExpYear { get; set; }
    }
}