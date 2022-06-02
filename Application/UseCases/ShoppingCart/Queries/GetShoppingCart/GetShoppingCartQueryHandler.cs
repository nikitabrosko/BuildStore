using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UseCases.ShoppingCart.Queries.GetShoppingCart
{
    public class GetShoppingCartQueryHandler : IRequestHandler<GetShoppingCartQuery, Domain.Entities.ShoppingCart>
    {
        private readonly IApplicationDbContext _context;

        public GetShoppingCartQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Domain.Entities.ShoppingCart> Handle(GetShoppingCartQuery request, CancellationToken cancellationToken)
        {
            var shoppingCartEntity = await _context.ShoppingCarts
                .Include(s => s.ProductsDictionary)
                .ThenInclude(p => p.Product)
                .ThenInclude(p => p.Images)
                .SingleOrDefaultAsync(s => s.Id.Equals(request.Id), cancellationToken);

            if (shoppingCartEntity is null)
            {
                throw new NotFoundException(nameof(Domain.Entities.ShoppingCart), request.Id);
            }

            return shoppingCartEntity;
        }
    }
}