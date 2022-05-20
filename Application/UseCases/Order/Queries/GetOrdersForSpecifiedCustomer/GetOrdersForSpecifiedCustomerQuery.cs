using Application.Common.Models;
using MediatR;

namespace Application.UseCases.Order.Queries.GetOrdersForSpecifiedCustomer
{
    public class GetOrdersForSpecifiedCustomerQuery : IRequest<PaginatedList<Domain.Entities.Order>>
    {
        public int CustomerId { get; set; }

        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }
}