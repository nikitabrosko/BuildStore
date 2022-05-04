using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UseCases.Subcategory.Commands.AddSubcategory
{
    public class AddSubcategoryCommandHandler : IRequestHandler<AddSubcategoryCommand, int>
    {
        private readonly IApplicationDbContext _context;

        public AddSubcategoryCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(AddSubcategoryCommand request, CancellationToken cancellationToken)
        {
            var subcategory = await _context.Subcategories
                .Include(s => s.Subcategories)
                .Include(s => s.Category)
                .SingleOrDefaultAsync(s => s.Id.Equals(request.SubcategoryId), cancellationToken);

            if (subcategory is null)
            {
                throw new NotFoundException(nameof(Subcategory), request.SubcategoryId);
            }

            var entity = new Domain.Entities.Subcategory
            {
                Category = subcategory.Category,
                Name = request.Name,
                Description = request.Description
            };

            var checkForExistsEntity = _context.Subcategories
                .Any(s => s.Name.Equals(entity.Name));

            if (checkForExistsEntity)
            {
                throw new ItemExistsException($"{nameof(Domain.Entities.Subcategory)} with this name is already exists!");
            }

            await _context.Subcategories.AddAsync(entity, cancellationToken);

            subcategory.Subcategories.Add(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return subcategory.Category.Id;
        }
    }
}