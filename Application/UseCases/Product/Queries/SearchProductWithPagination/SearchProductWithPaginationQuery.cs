using Application.Common.Models;
using MediatR;

namespace Application.UseCases.Product.Queries.SearchProductWithPagination
{
    public class SearchProductWithPaginationQuery : IRequest<PaginatedList<Domain.Entities.Product>>
    {
        public string Text { get; set; }

        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }
}