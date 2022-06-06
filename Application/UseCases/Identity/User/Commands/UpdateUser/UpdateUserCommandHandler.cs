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
                await _userManager.SetUserNameAsync(entity, request.Name);
                await _userManager.UpdateNormalizedUserNameAsync(entity);
            }

            if (request.Email != null)
            {
                await _userManager.SetEmailAsync(entity, request.Email);
                await _userManager.UpdateNormalizedEmailAsync(entity);
            }

            if (request.Password != null)
            {
                await _userManager.RemovePasswordAsync(entity);
                await _userManager.AddPasswordAsync(entity, request.Password);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}