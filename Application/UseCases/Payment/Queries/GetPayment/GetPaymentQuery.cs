using MediatR;

namespace Application.UseCases.Payment.Queries.GetPayment
{
    public class GetPaymentQuery : IRequest<Domain.Entities.Payment>
    {
        public int Id { get; set; }
    }
}