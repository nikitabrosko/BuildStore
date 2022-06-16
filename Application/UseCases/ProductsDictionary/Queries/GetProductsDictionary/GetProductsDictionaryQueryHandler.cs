using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UseCases.ProductsDictionary.Queries.GetProductsDictionary
{
    public class GetProductsDictionaryQueryHandler : IRequestHandler<GetProductsDictionaryQuery, Domain.Entities.ProductsDictionary>
    {
        private readonly IApplicationDbContext _context;

        public GetProductsDictionaryQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Domain.Entities.ProductsDictionary> Handle(GetProductsDictionaryQuery request, CancellationToken cancellationToken)
        {
            return await _context.ProductsDictionaries
                .Include(p => p.Product)
                    .ThenInclude(p => p.Images)
                .Include(p => p.ShoppingCart)
                .Include(p => p.Order)
                .SingleOrDefaultAsync(p => p.Id.Equals(request.Id), cancellationToken);
        }
    }
}
