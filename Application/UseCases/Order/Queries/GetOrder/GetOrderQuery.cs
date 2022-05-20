using MediatR;

namespace Application.UseCases.Order.Queries.GetOrder
{
    public class GetOrderQuery : IRequest<Domain.Entities.Order>
    {
        public int Id { get; set; }
    }
}