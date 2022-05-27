using System.Collections.Generic;
using Domain.Entities;
using MediatR;

namespace Application.UseCases.Order.Commands.CreateOrder
{
    public class CreateOrderCommand : IRequest<int>
    {
        public ICollection<ProductsDictionary> ProductsDictionary { get; set; }

        public Domain.Entities.Customer Customer { get; set; }

        public Domain.Entities.Delivery Delivery { get; set; }

        public Domain.Entities.Payment Payment { get; set; }
    }
}