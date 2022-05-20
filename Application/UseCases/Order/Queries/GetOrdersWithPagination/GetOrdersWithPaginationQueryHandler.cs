using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UseCases.Order.Queries.GetOrdersWithPagination
{
    public class GetOrdersWithPaginationQueryHandler : IRequestHandler<GetOrdersWithPaginationQuery, PaginatedList<Domain.Entities.Order>>
    {
        private readonly IApplicationDbContext _context;

        public GetOrdersWithPaginationQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PaginatedList<Domain.Entities.Order>> Handle(GetOrdersWithPaginationQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Orders
                .Include(o => o.Products);

            return await PaginatedList<Domain.Entities.Order>.CreateAsync(query, request.PageNumber, request.PageSize);
        }
    }
}