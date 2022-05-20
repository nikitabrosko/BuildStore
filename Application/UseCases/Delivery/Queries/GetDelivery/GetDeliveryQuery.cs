using MediatR;

namespace Application.UseCases.Delivery.Queries.GetDelivery
{
    public class GetDeliveryQuery : IRequest<Domain.Entities.Delivery>
    {
        public int Id { get; set; }
    }
}