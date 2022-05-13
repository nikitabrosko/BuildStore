using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using Domain.IdentityEntities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.UseCases.ShoppingCart.Queries.GetShoppingCart
{
    public class GetShoppingCartCommandHandler : IRequestHandler<GetShoppingCartCommand, Domain.Entities.ShoppingCart>
    {
        private readonly UserManager<User> _userManager;

        public GetShoppingCartCommandHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Domain.Entities.ShoppingCart> Handle(GetShoppingCartCommand request, CancellationToken cancellationToken)
        {
            var entity = (await _userManager.Users
                .Include(u => u.ShoppingCart)
                .ThenInclude(s => s.Products)
                .SingleOrDefaultAsync(s => s.UserName.Equals(request.Username), cancellationToken)).ShoppingCart;

            if (entity is null)
            {
                throw new NotFoundException(nameof(Domain.Entities.ShoppingCart), request.Username);
            }

            return entity;
        }
    }
}