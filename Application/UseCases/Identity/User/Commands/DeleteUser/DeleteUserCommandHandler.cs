using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.UseCases.Identity.User.Commands.DeleteUser
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
    {
        private readonly UserManager<Domain.IdentityEntities.User> _userManager;

        public DeleteUserCommandHandler(UserManager<Domain.IdentityEntities.User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var entity = await _userManager.FindByIdAsync(request.Id);

            if (entity is null)
            {
                throw new NotFoundException(nameof(User), request.Id);
            }

            await _userManager.DeleteAsync(entity);

            return Unit.Value;
        }
    }
}