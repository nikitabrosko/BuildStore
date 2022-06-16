using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UseCases.ShoppingCart.Commands.ClearProducts
{
    public class ClearProductsCommandHandler : IRequestHandler<ClearProductsCommand>
    {
        private readonly IApplicationDbContext _context;

        public ClearProductsCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(ClearProductsCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.ShoppingCarts
                .Include(s => s.ProductsDictionary)
                .ThenInclude(p => p.Order)
                .SingleOrDefaultAsync(s => s.Id.Equals(request.Id), cancellationToken);

            if (entity is null)
            {
                throw new NotFoundException(nameof(Domain.Entities.ShoppingCart), request.Id);
            }

            foreach (var productsDictionary in entity.ProductsDictionary)
            {
                productsDictionary.ShoppingCart = null;
            }

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}