using MediatR;

namespace Application.UseCases.Order.Commands.UpdateOrder
{
    public class UpdateOrderCommand : IRequest
    {
        public int Id { get; set; }

        public bool DeliveryFulfilled { get; set; }

        public bool PaymentAllowed { get; set; }
    }
}