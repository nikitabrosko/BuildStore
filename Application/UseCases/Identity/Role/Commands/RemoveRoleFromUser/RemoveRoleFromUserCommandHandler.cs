using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.UseCases.Identity.Role.Commands.RemoveRoleFromUser
{
    public class RemoveRoleFromUserCommandHandler : IRequestHandler<RemoveRoleFromUserCommand, IdentityResult>
    {
        private readonly UserManager<Domain.IdentityEntities.User> _userManager;

        public RemoveRoleFromUserCommandHandler(UserManager<Domain.IdentityEntities.User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IdentityResult> Handle(RemoveRoleFromUserCommand request, CancellationToken cancellationToken)
        {
            var entity = await _userManager.FindByIdAsync(request.UserId);

            if (entity is null)
            {
                throw new NotFoundException(nameof(Domain.IdentityEntities.User), request.UserId);
            }

            return await _userManager.RemoveFromRoleAsync(entity, request.RoleName);
        }
    }
}