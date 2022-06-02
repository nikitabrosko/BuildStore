using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UseCases.Product.Commands.DeleteProduct
{
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, int>
    {
        private readonly IApplicationDbContext _context;

        public DeleteProductCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Products
                .Include(p => p.Supplier)
                .Include(p => p.Images)
                .ThenInclude(i => i.Product)
                .SingleOrDefaultAsync(p => p.Id.Equals(request.Id), cancellationToken);

            if (entity is null)
            {
                throw new NotFoundException(nameof(Domain.Entities.Product), request.Id);
            }

            foreach (var image in entity.Images)
            {
                image.Product = null;
                _context.ProductImages.Remove(image);
            }

            entity.Images = null;

            var supplierId = entity.Supplier.Id;

            _context.Products.Remove(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return supplierId;
        }
    }
}