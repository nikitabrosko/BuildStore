using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UseCases.Supplier.Queries.GetSuppliers
{
    public class GetSuppliersQueryHandler : IRequestHandler<GetSuppliersQuery, IEnumerable<Domain.Entities.Supplier>>
    {
        private readonly IApplicationDbContext _context;

        public GetSuppliersQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Domain.Entities.Supplier>> Handle(GetSuppliersQuery request, CancellationToken cancellationToken)
        {
            return await _context.Suppliers.ToListAsync(cancellationToken);
        }
    }
}