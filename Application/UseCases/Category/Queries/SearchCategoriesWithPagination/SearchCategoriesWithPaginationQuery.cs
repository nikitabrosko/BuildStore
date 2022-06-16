using Application.Common.Models;
using MediatR;

namespace Application.UseCases.Category.Queries.SearchCategoriesWithPagination
{
    public class SearchCategoriesWithPaginationQuery : IRequest<PaginatedList<Domain.Entities.Category>>
    {
        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        public string Pattern { get; set; }
    }
}
