using Application.Common.Models;
using MediatR;

namespace Application.UseCases.Order.Queries.SearchOrdersWithPagination
{
    public class SearchOrdersWithPaginationQuery : IRequest<PaginatedList<Domain.Entities.Order>>
    {
        public string Pattern { get; set; }

        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }
}
