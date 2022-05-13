using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.UseCases.Identity.Role.Commands.AddRoleToUser
{
    public class AddRoleToUserCommand : IRequest<IdentityResult>
    {
        public string UserId { get; set; }

        public string RoleId { get; set; }
    }
}