using Application.Common.Models;
using MediatR;

namespace Application.UseCases.Product.Queries.GetProductsWithPagination
{
    public class GetProductsWithPaginationQuery : IRequest<PaginatedList<Domain.Entities.Product>>
    {
        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }
}
