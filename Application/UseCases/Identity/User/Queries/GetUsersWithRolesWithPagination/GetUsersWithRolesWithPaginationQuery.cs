using Application.Common.Models;
using MediatR;

namespace Application.UseCases.Identity.User.Queries.GetUsersWithRolesWithPagination
{
    public class GetUsersWithRolesWithPaginationQuery : IRequest<PaginatedList<UserWithRolesDto>>
    {
        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }
}