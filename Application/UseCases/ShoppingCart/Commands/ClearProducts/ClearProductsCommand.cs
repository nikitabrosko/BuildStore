using MediatR;

namespace Application.UseCases.ShoppingCart.Commands.ClearProducts
{
    public class ClearProductsCommand : IRequest
    {
        public int Id { get; set; }
    }
}