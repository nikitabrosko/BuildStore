using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UseCases.Identity.Role.Queries.GetRole
{
    public class GetRoleQueryHandler : IRequestHandler<GetRoleQuery, IdentityRole>
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public GetRoleQueryHandler(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<IdentityRole> Handle(GetRoleQuery request, CancellationToken cancellationToken)
        {
            if (request.Id is null)
            {
                return await _roleManager.FindByNameAsync(request.Name);
            }
            else if (request.Name is null)
            {
                return await _roleManager.FindByIdAsync(request.Id);
            }

            throw new ArgumentException("Both id and Name cannot be in one query!");
        }
    }
}
