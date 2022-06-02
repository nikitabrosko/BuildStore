using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UseCases.Product.Queries.SearchProductWithPagination
{
    public class SearchProductWithPaginationQueryHandler : IRequestHandler<SearchProductWithPaginationQuery, PaginatedList<Domain.Entities.Product>>
    {
        private readonly IApplicationDbContext _context;

        public SearchProductWithPaginationQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PaginatedList<Domain.Entities.Product>> Handle(SearchProductWithPaginationQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Products
                .Include(p => p.Images)
                .Where(p => p.Name.StartsWith(request.Text));

            return await PaginatedList<Domain.Entities.Product>.CreateAsync(query, request.PageNumber, request.PageSize);
        }
    }
}