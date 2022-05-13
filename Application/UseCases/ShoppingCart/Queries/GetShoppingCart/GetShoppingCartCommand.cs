using MediatR;

namespace Application.UseCases.ShoppingCart.Queries.GetShoppingCart
{
    public class GetShoppingCartCommand : IRequest<Domain.Entities.ShoppingCart>
    {
        public string Username { get; set; }
    }
}