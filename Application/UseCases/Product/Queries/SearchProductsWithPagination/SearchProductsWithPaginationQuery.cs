using Application.Common.Models;
using MediatR;

namespace Application.UseCases.Product.Queries.SearchProductsWithPagination
{
    public class SearchProductsWithPaginationQuery : IRequest<PaginatedList<Domain.Entities.Product>>
    {
        public string Pattern { get; set; }

        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }
}