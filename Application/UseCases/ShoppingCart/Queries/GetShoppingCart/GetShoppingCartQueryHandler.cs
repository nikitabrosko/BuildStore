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
        private readonly IApplicationIdentityDbContext _identityContext;

        public GetShoppingCartQueryHandler(IApplicationDbContext context, IApplicationIdentityDbContext identityContext)
        {
            _context = context;
            _identityContext = identityContext;
        }

        public async Task<Domain.Entities.ShoppingCart> Handle(GetShoppingCartQuery request, CancellationToken cancellationToken)
        {
            var userEntity = await _identityContext.Users
                .Include(u => u.ShoppingCart)
                .SingleOrDefaultAsync(u => u.UserName.Equals(request.Username), cancellationToken);

            var shoppingCartEntity = await _context.ShoppingCarts
                .Include(s => s.Products)
                .SingleOrDefaultAsync(s => s.Id.Equals(userEntity.ShoppingCart.Id), cancellationToken);

            if (shoppingCartEntity is null)
            {
                throw new NotFoundException(nameof(Domain.Entities.ShoppingCart), request.Username);
            }

            return shoppingCartEntity;
        }
    }
}