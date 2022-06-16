using System.Threading;
using System.Threading.Tasks;
using Application.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.UseCases.Identity.User.Queries.GetUsersWithPagination
{
    public class GetUsersWithPaginationQueryHandler : IRequestHandler<GetUsersWithPaginationQuery, PaginatedList<Domain.IdentityEntities.User>>
    {
        private readonly UserManager<Domain.IdentityEntities.User> _userManager;

        public GetUsersWithPaginationQueryHandler(UserManager<Domain.IdentityEntities.User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<PaginatedList<Domain.IdentityEntities.User>> Handle(GetUsersWithPaginationQuery request, CancellationToken cancellationToken)
        {
            var query = _userManager.Users;

            return await PaginatedList<Domain.IdentityEntities.User>.CreateAsync(query, request.PageNumber, request.PageSize);
        }
    }
}