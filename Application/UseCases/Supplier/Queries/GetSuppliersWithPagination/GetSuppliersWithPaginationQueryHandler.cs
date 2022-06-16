using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UseCases.Supplier.Queries.GetSuppliersWithPagination
{
    public class GetSuppliersWithPaginationQueryHandler : IRequestHandler<GetSuppliersWithPaginationQuery, PaginatedList<Domain.Entities.Supplier>>
    {
        private readonly IApplicationDbContext _context;

        public GetSuppliersWithPaginationQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PaginatedList<Domain.Entities.Supplier>> Handle(GetSuppliersWithPaginationQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Suppliers
                .Include(s => s.Products);

            return await PaginatedList<Domain.Entities.Supplier>.CreateAsync(query, request.PageNumber, request.PageSize);
        }
    }
}
