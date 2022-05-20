using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UseCases.Order.Queries.GetOrdersForSpecifiedCustomer
{
    public class GetOrdersForSpecifiedCustomerQueryHandler : IRequestHandler<GetOrdersForSpecifiedCustomerQuery, PaginatedList<Domain.Entities.Order>>
    {
        private readonly IApplicationDbContext _context;

        public GetOrdersForSpecifiedCustomerQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PaginatedList<Domain.Entities.Order>> Handle(GetOrdersForSpecifiedCustomerQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Delivery)
                .Include(o => o.Payment)
                .Where(o => o.CustomerId.Equals(request.CustomerId));

            return await PaginatedList<Domain.Entities.Order>.CreateAsync(query, request.PageNumber, request.PageSize);
        }
    }
}