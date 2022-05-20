using Domain.Entities;
using MediatR;

namespace Application.UseCases.Delivery.Commands.CreateDelivery
{
    public class CreateDeliveryCommand : IRequest<int>
    {
        public DeliveryType Type { get; set; }

        public int OrderId { get; set; }
    }
}