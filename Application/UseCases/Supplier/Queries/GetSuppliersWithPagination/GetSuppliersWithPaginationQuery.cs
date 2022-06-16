using Application.Common.Models;
using MediatR;

namespace Application.UseCases.Supplier.Queries.GetSuppliersWithPagination
{
    public class GetSuppliersWithPaginationQuery : IRequest<PaginatedList<Domain.Entities.Supplier>>
    {
        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }
}
