using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UseCases.Category.Commands.DeleteCategory
{
    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand>
    {
        private readonly IApplicationDbContext _context;

        public DeleteCategoryCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Categories
                .Include(c => c.Products)
                .Include(c => c.Subcategories)
                .ThenInclude(c => c.Subcategories)
                .SingleOrDefaultAsync(c => c.Id.Equals(request.Id), cancellationToken);

            if (entity is null)
            {
                throw new NotFoundException(nameof(Category), request.Id);
            }

            if (request.SubcategoriesDeletion)
            {
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

                foreach (var entitySubcategory in entity.Subcategories)
                {
                    _context.Categories.RemoveRange(entitySubcategory.Subcategories);
                }

                _context.Categories.RemoveRange(entity.Subcategories);
            }
            else
            {
                if (request.ProductsDeletion)
                {
                    _context.Products.RemoveRange(entity.Products);
                }

                foreach (var entitySubcategories in entity.Subcategories)
                {
                    entitySubcategories.Category = null;
                }
            }

            _context.Categories.Remove(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}