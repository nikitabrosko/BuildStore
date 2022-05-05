using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UseCases.Subcategory.Commands.UpdateSubcategory
{
    public class UpdateSubcategoryCommandHandler : IRequestHandler<UpdateSubcategoryCommand, int>
    {
        private readonly IApplicationDbContext _context;

        public UpdateSubcategoryCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(UpdateSubcategoryCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Categories
                .OfType<Domain.Entities.Subcategory>()
                .Include(c => c.Category)
                .ThenInclude(c => (c as Domain.Entities.Subcategory).Category)
                .SingleOrDefaultAsync(c => c.Id.Equals(request.Id), cancellationToken);

            if (entity is null)
            {
                throw new NotFoundException(nameof(Subcategory), request.Id);
            }

            if (!request.Name.Equals(entity.Name))
            {
                var checkForExistsEntity = _context.Categories
                    .Any(category => category.Name.Equals(request.Name));

                if (checkForExistsEntity)
                {
                    throw new ItemExistsException($"{nameof(Subcategory)} with that name is already exists!");
                }
            }

            entity.Name = request.Name;
            entity.Description = request.Description;

            await _context.SaveChangesAsync(cancellationToken);

            if (entity.Category is Domain.Entities.Subcategory subcategory)
            {
                return subcategory.Category.Id;
            }

            return entity.Category.Id;
        }
    }
}