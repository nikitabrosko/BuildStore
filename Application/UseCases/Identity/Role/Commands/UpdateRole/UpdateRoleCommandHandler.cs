using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.UseCases.Identity.Role.Commands.UpdateRole
{
    public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand, IdentityResult>
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public UpdateRoleCommandHandler(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<IdentityResult> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
        {
            var entity = await _roleManager.FindByIdAsync(request.Id);

            if (entity is null)
            {
                throw new NotFoundException(nameof(IdentityRole), request.Id);
            }

            var result = await _roleManager.SetRoleNameAsync(entity, request.Name);

            return result;
        }
    }
}