using System.Threading;
using System.Threading.Tasks;
using Application.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.UseCases.Identity.Role.Queries.GetRolesWithPagination
{
    public class GetRolesWithPaginationQueryHandler : IRequestHandler<GetRolesWithPaginationQuery, PaginatedList<IdentityRole>>
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public GetRolesWithPaginationQueryHandler(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<PaginatedList<IdentityRole>> Handle(GetRolesWithPaginationQuery request, CancellationToken cancellationToken)
        {
            var query = _roleManager.Roles;

            return await PaginatedList<IdentityRole>.CreateAsync(query, request.PageNumber, request.PageSize);
        }
    }
}