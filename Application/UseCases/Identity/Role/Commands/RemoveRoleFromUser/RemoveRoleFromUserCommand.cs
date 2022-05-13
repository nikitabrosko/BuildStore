using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.UseCases.Identity.Role.Commands.RemoveRoleFromUser
{
    public class RemoveRoleFromUserCommand : IRequest<IdentityResult>
    {
        public string UserId { get; set; }

        public string RoleName { get; set; }
    }
}