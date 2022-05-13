using Application.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.UseCases.Identity.Role.Queries.GetRolesWithPagination
{
    public class GetRolesWithPaginationQuery : IRequest<PaginatedList<IdentityRole>>
    {
        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }
}