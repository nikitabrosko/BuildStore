using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.UseCases.Identity.Role.Commands.DeleteRole
{
    public class DeleteRoleCommand : IRequest<IdentityResult>
    {
        public string Id { get; set; }
    }
}