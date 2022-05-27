using System.Collections.Generic;
using System.Linq;
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
                .Include(s => s.ProductsDictionary)
                .SingleOrDefaultAsync(s => s.Id.Equals(userEntity.ShoppingCart.Id), cancellationToken);

            var productEntity = await _context.Products
                .SingleOrDefaultAsync(p => p.Id.Equals(request.ProductId), cancellationToken);

            if (productEntity is null)
            {
                throw new NotFoundException(nameof(Domain.Entities.Product), request.ProductId);
            }

            var productDictionaryEntity = await _context.ProductsDictionaries
                .Include(p => p.Product)
                .Include(p => p.ShoppingCart)
                .SingleOrDefaultAsync(p => p.Product.Equals(productEntity) 
                                           && p.ShoppingCart.Id.Equals(shoppingCartEntity.Id), cancellationToken);

            if (productDictionaryEntity is null)
            {
                await _context.ProductsDictionaries.AddAsync(new Domain.Entities.ProductsDictionary
                    {Product = productEntity, Count = 1, ShoppingCart = shoppingCartEntity}, cancellationToken);
            }
            else
            {
                shoppingCartEntity.ProductsDictionary.Single(p => p.Equals(productDictionaryEntity)).Count += 1;
            }

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}