using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UseCases.Subcategory.Queries.GetSubcategoriesWithPagination
{
    public class GetSubcategoriesWithPaginationQueryHandler : IRequestHandler<GetSubcategoriesWithPaginationQuery, PaginatedList<Domain.Entities.Subcategory>>
    {
        private readonly IApplicationDbContext _context;

        public GetSubcategoriesWithPaginationQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PaginatedList<Domain.Entities.Subcategory>> Handle(GetSubcategoriesWithPaginationQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Categories
                .OfType<Domain.Entities.Subcategory>()
                .Include(s => s.Category)
                .Include(s => s.Subcategories)
                .Include(s => s.Products);

            return await PaginatedList<Domain.Entities.Subcategory>.CreateAsync(query, request.PageNumber, request.PageSize);
        }
    }
}
