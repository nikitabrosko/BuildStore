using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.UseCases.Identity.User.Commands.UpdateUser
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand>
    {
        private readonly IApplicationIdentityDbContext _context;
        private readonly UserManager<Domain.IdentityEntities.User> _userManager;

        public UpdateUserCommandHandler(UserManager<Domain.IdentityEntities.User> userManager, IApplicationIdentityDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var entity = await _userManager.FindByIdAsync(request.Id);

            if (entity is null)
            {
                throw new NotFoundException(nameof(User), request.Id);
            }

            if (request.Name != null)
            {
                entity.UserName = request.Name;
            }

            if (request.Email != null)
            {
                entity.Email = request.Email;
            }

            if (request.Password != null)
            {
                await _userManager.ChangePasswordAsync(entity, entity.PasswordHash, request.Password);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}