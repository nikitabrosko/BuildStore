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
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, int>
    {
        private readonly IApplicationDbContext _context;

        public CreateProductCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateProductCommand request, CancellationToken cancellationToken)
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

            if (request.CategoryName != null)
            {
                var category = await _context.Categories
                    .Include(c => c.Products)
                    .SingleOrDefaultAsync(c => c.Name.Equals(request.CategoryName), cancellationToken);

                if (category is null)
                {
                    throw new NotFoundException(nameof(Category), request.CategoryName);
                }

                entity.Category = category;
            }

            await _context.Products.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return request.CategoryName != null ? entity.Category.Id : entity.Supplier.Id;
        }
    }
}