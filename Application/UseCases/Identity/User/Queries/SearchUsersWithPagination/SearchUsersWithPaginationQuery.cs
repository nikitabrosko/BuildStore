using Application.Common.Models;
using MediatR;

namespace Application.UseCases.Identity.User.Queries.SearchUsersWithPagination
{
    public class SearchUsersWithPaginationQuery : IRequest<PaginatedList<Domain.IdentityEntities.User>>
    {
        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        public string Pattern { get; set; }
    }
}
