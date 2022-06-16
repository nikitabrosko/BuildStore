using Application.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UseCases.Identity.Role.Queries.SearchRolesWithPagination
{
    public class SearchRolesWithPaginationQueryHandler : IRequestHandler<SearchRolesWithPaginationQuery, PaginatedList<IdentityRole>>
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public SearchRolesWithPaginationQueryHandler(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<PaginatedList<IdentityRole>> Handle(SearchRolesWithPaginationQuery request, CancellationToken cancellationToken)
        {
            var query = _roleManager.Roles
                .Where(r => r.Name.Contains(request.Pattern));

            return await PaginatedList<IdentityRole>.CreateAsync(query, request.PageNumber, request.PageSize);
        }
    }
}
