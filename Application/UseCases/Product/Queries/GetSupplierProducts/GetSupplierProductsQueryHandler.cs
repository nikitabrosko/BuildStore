using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UseCases.Product.Queries.GetSupplierProducts
{
    public class GetSupplierProductsQueryHandler : IRequestHandler<GetSupplierProductsQuery, IEnumerable<Domain.Entities.Product>>
    {
        private readonly IApplicationDbContext _context;

        public GetSupplierProductsQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Domain.Entities.Product>> Handle(GetSupplierProductsQuery request, CancellationToken cancellationToken)
        {
            return (await _context.Suppliers
                .Include(s => s.Products)
                .ThenInclude(p => p.Images)
                .SingleOrDefaultAsync(s => s.Id.Equals(request.Supplier.Id), cancellationToken))
                .Products;
        }
    }
}
