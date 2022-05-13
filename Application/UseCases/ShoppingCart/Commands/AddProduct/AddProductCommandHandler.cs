using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.IdentityEntities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.UseCases.ShoppingCart.Commands.AddProduct
{
    public class AddProductCommandHandler : IRequestHandler<AddProductCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public AddProductCommandHandler(IApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<Unit> Handle(AddProductCommand request, CancellationToken cancellationToken)
        {
            var shoppingCartEntity = (await _userManager.Users
                .Include(u => u.ShoppingCart)
                .ThenInclude(s => s.Products)
                .SingleOrDefaultAsync(s => s.UserName.Equals(request.Username), cancellationToken)).ShoppingCart;

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