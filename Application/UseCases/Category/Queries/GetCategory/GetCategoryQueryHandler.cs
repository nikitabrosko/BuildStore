using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UseCases.Category.Queries.GetCategory
{
    public class GetCategoryQueryHandler : IRequestHandler<GetCategoryQuery, Domain.Entities.Category>
    {
        private readonly IApplicationDbContext _context;

        public GetCategoryQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Domain.Entities.Category> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context.Categories
                .Include(c => c.Subcategories)
                .ThenInclude(c => c.Subcategories)
                .SingleOrDefaultAsync(c => c.Id.Equals(request.Id), cancellationToken);

            if (entity is null)
            {
                throw new NotFoundException(nameof(Category), request.Id);
            }

            return entity;
        }
    }
}