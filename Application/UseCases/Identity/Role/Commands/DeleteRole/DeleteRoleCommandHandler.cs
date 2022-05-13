using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.UseCases.Identity.Role.Commands.DeleteRole
{
    public class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommand, IdentityResult>
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public DeleteRoleCommandHandler(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<IdentityResult> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
        {
            var entity = await _roleManager.FindByIdAsync(request.Id);

            if (entity is null)
            {
                throw new NotFoundException(nameof(IdentityRole), request.Id);
            }

            var result = await _roleManager.DeleteAsync(entity);

            return result;
        }
    }
}