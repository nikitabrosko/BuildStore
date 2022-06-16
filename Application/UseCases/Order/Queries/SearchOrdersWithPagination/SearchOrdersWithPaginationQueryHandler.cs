using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UseCases.Order.Queries.SearchOrdersWithPagination
{
    public class SearchOrdersWithPaginationQueryHandler : IRequestHandler<SearchOrdersWithPaginationQuery, PaginatedList<Domain.Entities.Order>>
    {
        private readonly IApplicationDbContext _context;

        public SearchOrdersWithPaginationQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PaginatedList<Domain.Entities.Order>> Handle(SearchOrdersWithPaginationQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Delivery)
                .Include(o => o.Payment)
                .Include(o => o.ProductsDictionary)
                .ThenInclude(o => o.Product);

            return await PaginatedList<Domain.Entities.Order>.CreateAsync(query, request.PageNumber, request.PageSize);
        }
    }
}
