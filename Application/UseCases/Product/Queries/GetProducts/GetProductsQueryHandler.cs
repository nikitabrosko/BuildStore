using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UseCases.Product.Queries.GetProducts
{
    public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, IEnumerable<Domain.Entities.Product>>
    {
        private readonly IApplicationDbContext _context;

        public GetProductsQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Domain.Entities.Product>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            if (request.Count is not null)
            {
                int count = (int)request.Count;

                if (_context.Products.Count() > count)
                {
                    return await _context.Products
                        .Include(p => p.Category)
                        .Include(p => p.Supplier)
                        .Include(p => p.Images)
                        .Take(count)
                        .ToListAsync();
                }
            }

            return await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .Include(p => p.Images)
                .ToListAsync();
        }
    }
}