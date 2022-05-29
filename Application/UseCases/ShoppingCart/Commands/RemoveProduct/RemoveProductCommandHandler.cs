using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.IdentityEntities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UseCases.ShoppingCart.Commands.RemoveProduct
{
    public class RemoveProductCommandHandler : IRequestHandler<RemoveProductCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IApplicationIdentityDbContext _identityContext;

        public RemoveProductCommandHandler(IApplicationDbContext context, IApplicationIdentityDbContext identityContext)
        {
            _context = context;
            _identityContext = identityContext;
        }

        public async Task<Unit> Handle(RemoveProductCommand request, CancellationToken cancellationToken)
        {
            var userEntity = await _identityContext.Users
                .Include(u => u.ShoppingCart)
                .SingleOrDefaultAsync(u => u.UserName.Equals(request.Username), cancellationToken);

            if (userEntity is null)
            {
                throw new NotFoundException(nameof(User), request.Username);
            }

            var shoppingCartEntity = await _context.ShoppingCarts
                .Include(s => s.ProductsDictionary)
                .SingleOrDefaultAsync(s => s.Id.Equals(userEntity.ShoppingCart.Id), cancellationToken);

            var productEntity = await _context.Products
                .SingleOrDefaultAsync(p => p.Id.Equals(request.ProductId), cancellationToken);

            if (productEntity is null)
            {
                throw new NotFoundException(nameof(Domain.Entities.Product), request.ProductId);
            }

            var productsDictionaryEntity = await _context.ProductsDictionaries
                .Include(p => p.Product)
                .Include(p => p.ShoppingCart)
                .SingleOrDefaultAsync(p => p.Product.Equals(productEntity)
                                           && p.ShoppingCart.Id.Equals(shoppingCartEntity.Id), cancellationToken);

            shoppingCartEntity.ProductsDictionary.Single(p => p.Equals(productsDictionaryEntity)).Count -= 1;

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}