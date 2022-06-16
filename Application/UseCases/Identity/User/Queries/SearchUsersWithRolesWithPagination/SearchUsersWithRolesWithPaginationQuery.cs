using Application.Common.Models;
using Application.UseCases.Identity.User.Queries.GetUsersWithRolesWithPagination;
using MediatR;

namespace Application.UseCases.Identity.User.Queries.SearchUsersWithRolesWithPagination
{
    public class SearchUsersWithRolesWithPaginationQuery : IRequest<PaginatedList<UserWithRolesDto>>
    {
        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        public string Pattern { get; set; }
    }
}
