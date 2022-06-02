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

            var category = await _context.Categories
                .Include(c => c.Products)
                .SingleOrDefaultAsync(c => c.Name.Equals(request.CategoryName), cancellationToken);

            if (category is null)
            {
                throw new NotFoundException(nameof(Domain.Entities.Subcategory), request.CategoryName);
            }

            entity.Name = request.Name;
            entity.Description = request.Description;
            entity.Price = request.Price;
            entity.QuantityPerUnit = request.QuantityPerUnit;
            entity.Weight = request.Weight;
            entity.Discount = request.Discount;
            entity.Category = category;

            var checkForExistsEntity = _context.Products.AsEnumerable()
                .SkipWhile(p => p.Id.Equals(request.Id))
                .Any(p => p.Name.Equals(entity.Name)
                          && p.Supplier.Equals(entity.Supplier));

            if (checkForExistsEntity)
            {
                throw new ItemExistsException(
                    $"{nameof(Domain.Entities.Product)} with this name and supplier is already exists!");
            }

            if (request.Pictures != null)
            {
                for (int i = 0; i < request.Pictures.Length; i++)
                {
                    if (request.Pictures != null)
                    {
                        entity.Images.ToList()[i].Picture = new byte[request.Pictures[i].Length];

                        await using (var stream = request.Pictures[i].OpenReadStream())
                        {
                            var count = stream.Read(entity.Images.ToList()[i].Picture, 0, (int)request.Pictures[i].Length);
                        }

                        _context.ProductImages.Update(entity.Images.ToList()[i]);
                    }
                }
            }

            await _context.SaveChangesAsync(cancellationToken);

            return entity.Supplier.Id;
        }
    }
}