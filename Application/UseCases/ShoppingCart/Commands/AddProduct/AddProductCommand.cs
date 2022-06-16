using MediatR;

namespace Application.UseCases.ShoppingCart.Commands.AddProduct
{
    public class AddProductCommand : IRequest
    {
        public int ProductId { get; set; }

        public string Username { get; set; }

        public int Amount { get; set; } = 1;
    }
}