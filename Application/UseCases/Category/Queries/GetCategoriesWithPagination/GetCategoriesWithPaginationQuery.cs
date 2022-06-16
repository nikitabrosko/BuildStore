using Application.Common.Models;
using MediatR;

namespace Application.UseCases.Category.Queries.GetCategoriesWithPagination
{
    public class GetCategoriesWithPaginationQuery : IRequest<PaginatedList<Domain.Entities.Category>>
    {
        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }
}
