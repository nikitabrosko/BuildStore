using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UseCases.Order.Commands.DeleteOrder
{
    public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand>
    {
        private readonly IApplicationDbContext _context;

        public DeleteOrderCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Orders
                .Include(o => o.Delivery)
                .Include(o => o.Payment)
                .Include(o => o.Products)
                .SingleOrDefaultAsync(o => o.Id.Equals(request.Id), cancellationToken);

            if (entity is null)
            {
                throw new NotFoundException(nameof(Domain.Entities.Order), request.Id);
            }

            if (entity.Delivery != null)
            {
                _context.Deliveries.Remove(entity.Delivery);
            }

            if (entity.Payment != null)
            {
                _context.Payments.Remove(entity.Payment);
            }

            entity.Products = null;

            _context.Orders.Remove(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}