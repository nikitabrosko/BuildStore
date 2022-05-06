using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UseCases.Supplier.Queries.GetSupplier
{
    public class GetSupplierQueryHandler : IRequestHandler<GetSupplierQuery, Domain.Entities.Supplier>
    {
        private readonly IApplicationDbContext _context;

        public GetSupplierQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Domain.Entities.Supplier> Handle(GetSupplierQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context.Suppliers
                .Include(s => s.Products)
                .SingleOrDefaultAsync(s => s.Id.Equals(request.Id), cancellationToken);

            if (entity is null)
            {
                throw new NotFoundException(nameof(Domain.Entities.Supplier), request.Id);
            }

            return entity;
        }
    }
}