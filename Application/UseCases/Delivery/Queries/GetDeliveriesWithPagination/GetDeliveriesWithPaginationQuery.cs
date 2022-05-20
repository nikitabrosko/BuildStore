using Application.Common.Models;
using MediatR;

namespace Application.UseCases.Delivery.Queries.GetDeliveriesWithPagination
{
    public class GetDeliveriesWithPaginationQuery : IRequest<PaginatedList<Domain.Entities.Delivery>>
    {
        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }
}