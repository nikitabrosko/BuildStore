using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UseCases.Product.Queries.SearchProductsWithPagination
{
    public class SearchProductsWithPaginationQueryHandler : IRequestHandler<SearchProductsWithPaginationQuery, PaginatedList<Domain.Entities.Product>>
    {
        private readonly IApplicationDbContext _context;

        public SearchProductsWithPaginationQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PaginatedList<Domain.Entities.Product>> Handle(SearchProductsWithPaginationQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Products
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .Include(p => p.Images)
                .Where(p => p.Name.Contains(request.Pattern));

            return await PaginatedList<Domain.Entities.Product>.CreateAsync(query, request.PageNumber, request.PageSize);
        }
    }
}