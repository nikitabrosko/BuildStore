using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UseCases.Product.Commands.UpdateProduct
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, int>
    {
        private readonly IApplicationDbContext _context;

        public UpdateProductCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Products
                .Include(p => p.Supplier)
                .SingleOrDefaultAsync(p => p.Id.Equals(request.Id), cancellationToken);

            if (entity is null)
            {
                throw new NotFoundException(nameof(Domain.Entities.Product), request.Id);
            }

            entity.Name = request.Name;
            entity.Description = request.Description;
            entity.Price = request.Price;
            entity.QuantityPerUnit = request.QuantityPerUnit;
            entity.Weight = request.Weight;
            entity.Discount = request.Discount;

            if (request.Picture != null)
            {
                entity.Picture = new byte[request.Picture.Length];

                await using var stream = request.Picture.OpenReadStream();
                var count = stream.Read(entity.Picture, 0, (int)request.Picture.Length);
            }

            var checkForExistsEntity = _context.Products.AsEnumerable()
                .SkipWhile(p => p.Id.Equals(request.Id))
                .Any(p => p.Name.Equals(entity.Name)
                               && p.Supplier.Equals(entity.Supplier));

            if (checkForExistsEntity)
            {
                throw new ItemExistsException(
                    $"{nameof(Domain.Entities.Product)} with this name and supplier is already exists!");
            }

            await _context.SaveChangesAsync(cancellationToken);

            return entity.Supplier.Id;
        }
    }
}