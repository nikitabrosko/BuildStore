using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UseCases.Subcategory.Commands.DeleteSubcategory
{
    public class DeleteSubcategoryCommandHandler : IRequestHandler<DeleteSubcategoryCommand, int>
    {
        private readonly IApplicationDbContext _context;

        public DeleteSubcategoryCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(DeleteSubcategoryCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Categories
                .OfType<Domain.Entities.Subcategory>()
                .Include(c => c.Category)
                .Include(c => (c.Category as Domain.Entities.Subcategory).Category)
                .Include(c => c.Products)
                .Include(c => c.Subcategories)
                .SingleOrDefaultAsync(c => c.Id.Equals(request.Id), cancellationToken);

            if (entity is null)
            {
                throw new NotFoundException(nameof(Subcategory), request.Id);
            }

            if (request.ProductsDeletion)
            {
                _context.Products.RemoveRange(entity.Products);
            }
            else
            {
                foreach (var entityProduct in entity.Products)
                {
                    entityProduct.Category = null;
                }
            }

            if (request.SubcategoriesDeletion)
            {
                _context.Categories.RemoveRange(entity.Subcategories);
            }
            else
            {
                foreach (var entitySubcategories in entity.Subcategories)
                {
                    entitySubcategories.Category = null;
                }
            }

            int categoryId;

            if (entity.Category is Domain.Entities.Subcategory subcategory)
            {
                categoryId = subcategory.Category.Id;
            }
            else
            {
                categoryId = entity.Category.Id;
            }

            _context.Categories.Remove(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return categoryId;
        }
    }
}