using Application.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UseCases.Identity.User.Queries.SearchUsersWithPagination
{
    public class SearchUsersWithPaginationQueryHandler : IRequestHandler<SearchUsersWithPaginationQuery, PaginatedList<Domain.IdentityEntities.User>>
    {
        private readonly UserManager<Domain.IdentityEntities.User> _userManager;

        public SearchUsersWithPaginationQueryHandler(UserManager<Domain.IdentityEntities.User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<PaginatedList<Domain.IdentityEntities.User>> Handle(SearchUsersWithPaginationQuery request, CancellationToken cancellationToken)
        {
            var query = _userManager.Users
                .Where(u => u.UserName.Contains(request.Pattern));

            return await PaginatedList<Domain.IdentityEntities.User>.CreateAsync(query, request.PageNumber, request.PageSize);
        }
    }
}
