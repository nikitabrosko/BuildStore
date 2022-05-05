using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UseCases.Category.Commands.DeleteCategory
{
    public class DeleteCategoryHandler : IRequestHandler<DeleteCategoryCommand>
    {
        private readonly IApplicationDbContext _context;

        public DeleteCategoryHandler(IApplicationDbContext context)
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

            if (request.SubcategoriesDeletion)
            {
                if (request.ProductsDeletion)
                {
                    _context.Products.RemoveRange(entity.Products);
                }
                else
                {
                    await entity.Products
                        .AsQueryable()
                        .ForEachAsync(p => p.Category = null, cancellationToken);
                }

                foreach (var entitySubcategory in entity.Subcategories)
                {
                    _context.Subcategories.RemoveRange(entitySubcategory.Subcategories);
                }

                _context.Subcategories.RemoveRange(entity.Subcategories);
            }
            else
            {
                if (request.ProductsDeletion)
                {
                    _context.Products.RemoveRange(entity.Products);
                }

                await entity.Subcategories
                    .AsQueryable()
                    .ForEachAsync(c => c.Category = null, cancellationToken);
            }

            _context.Categories.Remove(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}