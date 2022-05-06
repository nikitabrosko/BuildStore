using MediatR;

namespace Application.UseCases.Product.Queries.GetProduct
{
    public class GetProductQuery : IRequest<Domain.Entities.Product>
    {
        public int Id { get; set; }
    }
}