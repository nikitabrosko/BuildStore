using MediatR;

namespace Application.UseCases.Delivery.Commands.DeleteDelivery
{
    public class DeleteDeliveryCommand : IRequest
    {
        public int? Id { get; set; }
    }
}