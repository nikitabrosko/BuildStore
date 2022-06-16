using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UseCases.Subcategory.Queries.SearchSubcategoriesWithPagination
{
    public class SearchSubcategoriesWithPaginationQueryHandler : IRequestHandler<SearchSubcategoriesWithPaginationQuery, PaginatedList<Domain.Entities.Subcategory>>
    {
        private readonly IApplicationDbContext _context;

        public SearchSubcategoriesWithPaginationQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PaginatedList<Domain.Entities.Subcategory>> Handle(SearchSubcategoriesWithPaginationQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Categories
                .OfType<Domain.Entities.Subcategory>()
                .Include(c => c.Subcategories)
                .Include(c => c.Products)
                .Where(c => c.Name.Contains(request.Pattern));

            return await PaginatedList<Domain.Entities.Subcategory>.CreateAsync(query, request.PageNumber, request.PageSize);
        }
    }
}
