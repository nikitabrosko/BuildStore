using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UseCases.Product.Commands.CreateProduct
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand>
    {
        private readonly IApplicationDbContext _context;

        public CreateProductCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var supplier = await _context.Suppliers
                .Include(s => s.Products)
                .SingleOrDefaultAsync(s => s.Id.Equals(request.SupplierId), cancellationToken);

            if (supplier is null)
            {
                throw new NotFoundException(nameof(Supplier), request.SupplierId);
            }

            var entity = new Domain.Entities.Product
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                QuantityPerUnit = request.QuantityPerUnit,
                Weight = request.Weight,
                Discount = request.Discount,
                Supplier = supplier
            };

            var productsImages = new ProductImage[request.Pictures.Length];

            for (int i = 0; i < request.Pictures.Length; i++)
            {
                productsImages[i] = new ProductImage
                {
                    Picture = new byte[request.Pictures[i].Length],
                    Product = entity
                };

                await using (var stream = request.Pictures[i].OpenReadStream())
                {
                    var count = stream.Read(productsImages[i].Picture, 0, (int)request.Pictures[i].Length);
                }

                await _context.ProductImages.AddAsync(productsImages[i], cancellationToken);
            }

            entity.Images = productsImages;

            var checkForExistsEntity = await _context.Products
                .AnyAsync(p => p.Name.Equals(entity.Name) 
                               && p.Supplier.Equals(entity.Supplier), cancellationToken);

            if (checkForExistsEntity)
            {
                throw new ItemExistsException(
                    $"{nameof(Domain.Entities.Product)} with this name and supplier is already exists!");
            }

            var category = await _context.Categories
                .Include(c => c.Products)
                .SingleOrDefaultAsync(c => c.Id.Equals(request.CategoryId), cancellationToken);

            if (category is null)
            {
                throw new NotFoundException(nameof(Category), request.CategoryId);
            }

            entity.Category = category;

            await _context.Products.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}