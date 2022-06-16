using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UseCases.Category.Queries.GetCategoriesWithPagination
{
    public class GetCategoriesWithPaginationQueryHandler : IRequestHandler<GetCategoriesWithPaginationQuery, PaginatedList<Domain.Entities.Category>>
    {
        private readonly IApplicationDbContext _context;

        public GetCategoriesWithPaginationQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PaginatedList<Domain.Entities.Category>> Handle(GetCategoriesWithPaginationQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Categories
                .OfType<Domain.Entities.Category>()
                .Include(c => c.Subcategories)
                .Include(c => c.Products);

            return await PaginatedList<Domain.Entities.Category>.CreateAsync(query, request.PageNumber, request.PageSize);
        }
    }
}
