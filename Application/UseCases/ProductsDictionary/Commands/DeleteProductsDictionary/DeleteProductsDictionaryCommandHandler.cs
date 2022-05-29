using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UseCases.ProductsDictionary.Commands.DeleteProductsDictionary
{
    public class DeleteProductsDictionaryCommandHandler : IRequestHandler<DeleteProductsDictionaryCommand>
    {
        private readonly IApplicationDbContext _context;

        public DeleteProductsDictionaryCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteProductsDictionaryCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.ProductsDictionaries
                .Include(p => p.Order)
                .Include(p => p.Product)
                .Include(p => p.ShoppingCart)
                .SingleOrDefaultAsync(p => p.Id.Equals(request.Id), cancellationToken);

            if (entity is null)
            {
                throw new NotFoundException(nameof(ProductsDictionary), request.Id);
            }

            entity.Order = null;
            entity.Product = null;
            entity.ShoppingCart = null;

            _context.ProductsDictionaries.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}