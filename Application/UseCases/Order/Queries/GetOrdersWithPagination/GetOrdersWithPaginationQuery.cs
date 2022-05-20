using Application.Common.Models;
using MediatR;

namespace Application.UseCases.Order.Queries.GetOrdersWithPagination
{
    public class GetOrdersWithPaginationQuery : IRequest<PaginatedList<Domain.Entities.Order>>
    {
        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }
}