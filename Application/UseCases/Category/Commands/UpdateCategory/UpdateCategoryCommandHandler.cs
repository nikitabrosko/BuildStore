using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using MediatR;

namespace Application.UseCases.Category.Commands.UpdateCategory
{
    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand>
    {
        private readonly IApplicationDbContext _context;

        public UpdateCategoryCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Categories
                .FindAsync(new object[] {request.Id}, cancellationToken);

            if (entity is null)
            {
                throw new NotFoundException(nameof(Category), request.Id);
            }

            var checkForExistsEntity = _context.Categories
                .Any(category => category.Name.Equals(request.Name));

            if (checkForExistsEntity)
            {
                throw new ItemExistsException($"{nameof(Category)} with that name is already exists!");
            }

            entity.Name = request.Name;
            entity.Description = request.Description;
            entity.Subcategories.Add(request.Subcategory);

            return Unit.Value;
        }
    }
}