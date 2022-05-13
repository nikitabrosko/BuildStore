using MediatR;

namespace Application.UseCases.ShoppingCart.Commands.AddProduct
{
    public class AddProductCommand : IRequest
    {
        public int ProductId { get; set; }

        public string Username { get; set; }
    }
}