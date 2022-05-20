using System.Collections.Generic;
using MediatR;

namespace Application.UseCases.Order.Commands.CreateOrder
{
    public class CreateOrderCommand : IRequest<int>
    {
        public ICollection<Domain.Entities.Product> Products { get; set; }

        public Domain.Entities.Customer Customer { get; set; }

        public Domain.Entities.Delivery Delivery { get; set; }

        public Domain.Entities.Payment Payment { get; set; }
    }
}