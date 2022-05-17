using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.UseCases.Identity.User.Commands.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserCreatingResult>
    {
        private readonly IApplicationDbContext _context;
        private readonly UserManager<Domain.IdentityEntities.User> _manager;

        public CreateUserCommandHandler(IApplicationDbContext context, 
            UserManager<Domain.IdentityEntities.User> manager)
        {
            _context = context;
            _manager = manager;
        }

        public async Task<UserCreatingResult> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var entity = new Domain.IdentityEntities.User
            {
                UserName = request.Name,
                Email = request.Email
            };

            var shoppingCart = new Domain.Entities.ShoppingCart();

            await _context.ShoppingCarts.AddAsync(shoppingCart, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            entity.ShoppingCart = shoppingCart;

            var result = await _manager.CreateAsync(entity, request.Password); 
            await _manager.AddToRoleAsync(entity, "user");

            return new UserCreatingResult
            {
                Result = result,
                User = entity
            };
        }
    }
}