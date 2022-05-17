using MediatR;

namespace Application.UseCases.ShoppingCart.Commands.RemoveProduct
{
    public class RemoveProductCommand : IRequest
    {
        public int ProductId { get; set; }

        public string Username { get; set; }
    }
}