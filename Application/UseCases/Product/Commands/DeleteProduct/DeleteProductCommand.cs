using MediatR;

namespace Application.UseCases.Product.Commands.DeleteProduct
{
    public class DeleteProductCommand : IRequest<int>
    {
        public int Id { get; set; }
    }
}