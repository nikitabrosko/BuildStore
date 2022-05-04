using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UseCases.Subcategory.Commands.CreateSubcategory
{
    public class CreateSubcategoryCommandHandler : IRequestHandler<CreateSubcategoryCommand>
    {
        private readonly IApplicationDbContext _context;

        public CreateSubcategoryCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(CreateSubcategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _context.Categories
                .Include(s => s.Subcategories)
                .SingleOrDefaultAsync(s => s.Id.Equals(request.CategoryId), cancellationToken);

            if (category is null)
            {
                throw new NotFoundException(nameof(Category), request.CategoryId);
            }

            var entity = new Domain.Entities.Subcategory
            {
                Name = request.Name,
                Description = request.Description,
                Category = category
            };

            var checkForExistsEntity = _context.Subcategories
                .Any(subcategory => subcategory.Name.Equals(entity.Name));

            if (checkForExistsEntity)
            {
                throw new ItemExistsException($"{nameof(Domain.Entities.Subcategory)} with this name is already exists!");
            }

            await _context.Subcategories.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}