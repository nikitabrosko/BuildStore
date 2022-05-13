using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.UseCases.Identity.Role.Commands.UpdateRole
{
    public class UpdateRoleCommand : IRequest<IdentityResult>
    {
        public string Id { get; set; }

        public string Name { get; set; }
    }
}