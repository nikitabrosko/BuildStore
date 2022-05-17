using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UseCases.ShoppingCart.Commands.AddProduct
{
    public class AddProductCommandHandler : IRequestHandler<AddProductCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IApplicationIdentityDbContext _identityContext;

        public AddProductCommandHandler(IApplicationDbContext context, IApplicationIdentityDbContext identityContext)
        {
            _context = context;
            _identityContext = identityContext;
        }

        public async Task<Unit> Handle(AddProductCommand request, CancellationToken cancellationToken)
        {
            var userEntity = await _identityContext.Users
                .Include(u => u.ShoppingCart)
                .SingleOrDefaultAsync(u => u.UserName.Equals(request.Username), cancellationToken);

            var shoppingCartEntity = await _context.ShoppingCarts
                .Include(s => s.Products)
                .SingleOrDefaultAsync(s => s.Id.Equals(userEntity.ShoppingCart.Id), cancellationToken);

            var productEntity = await _context.Products
                .SingleOrDefaultAsync(p => p.Id.Equals(request.ProductId), cancellationToken);

            if (productEntity is null)
            {
                throw new NotFoundException(nameof(Domain.Entities.Product), request.ProductId);
            }

            shoppingCartEntity.Products.Add(productEntity);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}