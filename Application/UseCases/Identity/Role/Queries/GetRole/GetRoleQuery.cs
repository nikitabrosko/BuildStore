using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.UseCases.Identity.Role.Queries.GetRole
{
    public class GetRoleQuery : IRequest<IdentityRole>
    {
        public string Id { get; set; }

        public string Name { get; set; }
    }
}
