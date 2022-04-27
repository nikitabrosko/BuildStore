using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using MediatR;

namespace Application.UseCases.Category.Commands.CreateCategory
{
    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, int>
    {
        private readonly IApplicationDbContext _context;

        public CreateCategoryCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var entity = new Domain.Entities.Category
            {
                Name = request.Name,
                Description = request.Description,
                Picture = request.Picture
            };

            var checkForExistsEntity = _context.Categories
                .Any(category => category.Name.Equals(entity.Name));

            if (checkForExistsEntity)
            {
                throw new ItemExistsException($"{nameof(Domain.Entities.Category)} with this name is already exists!");
            }

            await _context.Categories.AddAsync(entity, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
    }
}