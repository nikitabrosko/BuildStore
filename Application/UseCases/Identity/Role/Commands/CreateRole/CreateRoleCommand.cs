using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.UseCases.Identity.Role.Commands.CreateRole
{
    public class CreateRoleCommand : IRequest<IdentityResult>
    {
        public string Name { get; set; }
    }
}