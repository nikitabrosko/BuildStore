using Application.Common.Models;
using MediatR;

namespace Application.UseCases.Identity.User.Queries.GetUsersWithPagination
{
    public class GetUsersWithPaginationQuery : IRequest<PaginatedList<Domain.IdentityEntities.User>>
    {
        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }
}