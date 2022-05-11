using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UseCases.Supplier.Commands.DeleteSupplier
{
    public class DeleteSupplierCommandHandler : IRequestHandler<DeleteSupplierCommand>
    {
        private readonly IApplicationDbContext _context;

        public DeleteSupplierCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteSupplierCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Suppliers
                .Include(s => s.Products)
                .SingleOrDefaultAsync(s => s.Id.Equals(request.Id), cancellationToken);

            if (entity is null)
            {
                throw new NotFoundException(nameof(Domain.Entities.Supplier), request.Id);
            }

            if (request.ProductsDeletion)
            {
                _context.Products.RemoveRange(entity.Products);
            }
            else
            {
                await _context.Products.ForEachAsync(p => p.Supplier = null, cancellationToken);
            }

            _context.Suppliers.Remove(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}