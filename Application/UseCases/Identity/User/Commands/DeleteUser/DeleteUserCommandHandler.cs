using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.UseCases.Identity.User.Commands.DeleteUser
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IApplicationIdentityDbContext _identityContext;
        private readonly UserManager<Domain.IdentityEntities.User> _userManager;

        public DeleteUserCommandHandler(IApplicationDbContext context, 
            IApplicationIdentityDbContext identityContext, 
            UserManager<Domain.IdentityEntities.User> userManager)
        {
            _context = context;
            _identityContext = identityContext;
            _userManager = userManager;
        }

        public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var entity = await _identityContext.Users
                .Include(u => u.ShoppingCart)
                .SingleOrDefaultAsync(u => u.Id.Equals(request.Id), cancellationToken);

            if (entity is null)
            {
                throw new NotFoundException(nameof(User), request.Id);
            }

            var shoppingCart = await
                _context.ShoppingCarts.SingleOrDefaultAsync(s => s.Id.Equals(entity.ShoppingCart.Id), cancellationToken);

            _context.ShoppingCarts.Remove(shoppingCart);
            await _context.SaveChangesAsync(cancellationToken);

            await _userManager.DeleteAsync(entity);

            return Unit.Value;
        }
    }
}