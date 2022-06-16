using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UseCases.Supplier.Queries.SearchSuppliersWithPagination
{
    public class SearchSuppliersWithPaginationQueryHandler : IRequestHandler<SearchSuppliersWithPaginationQuery, PaginatedList<Domain.Entities.Supplier>>
    {
        private readonly IApplicationDbContext _context;

        public SearchSuppliersWithPaginationQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PaginatedList<Domain.Entities.Supplier>> Handle(SearchSuppliersWithPaginationQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Suppliers
                .Include(c => c.Products)
                .Where(c => c.CompanyName.Contains(request.Pattern));

            return await PaginatedList<Domain.Entities.Supplier>.CreateAsync(query, request.PageNumber, request.PageSize);
        }
    }
}
