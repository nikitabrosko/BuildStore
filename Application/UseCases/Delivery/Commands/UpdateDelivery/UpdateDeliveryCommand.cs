using Domain.Entities;
using MediatR;

namespace Application.UseCases.Delivery.Commands.UpdateDelivery
{
    public class UpdateDeliveryCommand : IRequest
    {
        public int? Id { get; set; }

        public bool Fulfilled { get; set; }

        public DeliveryType Type { get; set; }
    }
}