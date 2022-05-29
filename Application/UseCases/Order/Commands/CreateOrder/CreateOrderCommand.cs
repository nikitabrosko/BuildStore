using System.Collections.Generic;
using Domain.Entities;
using MediatR;

namespace Application.UseCases.Order.Commands.CreateOrder
{
    public class CreateOrderCommand : IRequest<int>
    {
        public ICollection<Domain.Entities.ProductsDictionary> ProductsDictionary { get; set; }

        public Domain.Entities.Customer Customer { get; set; }

        public DeliveryType DeliveryType { get; set; }

        public PaymentType PaymentType { get; set; }

        public string CreditCardNumber { get; set; }

        public int CardExpMonth { get; set; }

        public int CardExpYear { get; set; }
    }
}