using Application.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.UseCases.Identity.Role.Queries.SearchRolesWithPagination
{
    public class SearchRolesWithPaginationQuery : IRequest<PaginatedList<IdentityRole>>
    {
        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        public string Pattern { get; set; }
    }
}
