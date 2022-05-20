using MediatR;

namespace Application.UseCases.Order.Commands.DeleteOrder
{
    public class DeleteOrderCommand : IRequest
    {
        public int Id { get; set; }
    }
}