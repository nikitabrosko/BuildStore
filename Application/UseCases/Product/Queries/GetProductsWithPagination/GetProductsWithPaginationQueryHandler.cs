using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UseCases.Product.Queries.GetProductsWithPagination
{
    public class GetProductsWithPaginationQueryHandler : IRequestHandler<GetProductsWithPaginationQuery, PaginatedList<Domain.Entities.Product>>
    {
        private readonly IApplicationDbContext _context;

        public GetProductsWithPaginationQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PaginatedList<Domain.Entities.Product>> Handle(GetProductsWithPaginationQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Products
                .Include(p => p.Images)
                .Include(p => p.Supplier)
                .Include(p => p.Category);

            return await PaginatedList<Domain.Entities.Product>.CreateAsync(query, request.PageNumber, request.PageSize);
        }
    }
}
