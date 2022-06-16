using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UseCases.Product.Queries.GetRelatedProducts
{
    public class GetRelatedProductsQueryHandler : IRequestHandler<GetRelatedProductsQuery, IEnumerable<Domain.Entities.Product>>
    {
        private readonly IApplicationDbContext _context;

        public GetRelatedProductsQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Domain.Entities.Product>> Handle(GetRelatedProductsQuery request, CancellationToken cancellationToken)
        {
            return await _context.Products
                .Include(p => p.Images)
                .Where(p => p.Name.Contains(request.Product.Name))
                .ToListAsync(cancellationToken);
        }
    }
}
