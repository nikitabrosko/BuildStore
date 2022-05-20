using MediatR;

namespace Application.UseCases.Order.Commands.UpdateOrder
{
    public class UpdateOrderCommand : IRequest
    {
        public int Id { get; set; }

        public Domain.Entities.Delivery Delivery { get; set; }

        public Domain.Entities.Payment Payment { get; set; }
    }
}