using Application.Common.Models;
using MediatR;

namespace Application.UseCases.Payment.Queries.GetPaymentsWithPagination
{
    public class GetPaymentsWithPaginationQuery : IRequest<PaginatedList<Domain.Entities.Payment>>
    {
        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }
}