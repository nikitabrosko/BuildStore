using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UseCases.Subcategory.Queries.GetSubcategories
{
    public class GetSubcategoriesQueryHandler : IRequestHandler<GetSubcategoriesQuery, IEnumerable<Domain.Entities.Subcategory>>
    {
        private readonly IApplicationDbContext _context;

        public GetSubcategoriesQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Domain.Entities.Subcategory>> Handle(GetSubcategoriesQuery request, CancellationToken cancellationToken)
        {
            return await _context.Categories
                .OfType<Domain.Entities.Subcategory>()
                .ToListAsync(cancellationToken);
        }
    }
}