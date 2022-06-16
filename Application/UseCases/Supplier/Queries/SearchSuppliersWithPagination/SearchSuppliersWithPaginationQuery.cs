using Application.Common.Models;
using MediatR;

namespace Application.UseCases.Supplier.Queries.SearchSuppliersWithPagination
{
    public class SearchSuppliersWithPaginationQuery : IRequest<PaginatedList<Domain.Entities.Supplier>>
    {
        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        public string Pattern { get; set; }
    }
}
