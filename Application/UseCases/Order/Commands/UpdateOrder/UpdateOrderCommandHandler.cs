using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UseCases.Order.Commands.UpdateOrder
{
    public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand>
    {
        private readonly IApplicationDbContext _context;

        public UpdateOrderCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Orders
                .Include(o => o.Delivery)
                .Include(o => o.Payment)
                .SingleOrDefaultAsync(o => o.Id.Equals(request.Id), cancellationToken);

            if (entity is null)
            {
                throw new NotFoundException(nameof(Domain.Entities.Order), request.Id);
            }

            if (request.Delivery != null)
            {
                entity.Delivery = request.Delivery;
                entity.DeliveryId = request.Delivery.Id;
            }

            if (request.Payment != null)
            {
                entity.Payment = request.Payment;
                entity.PaymentId = request.Payment.Id;
            }

            _context.Orders.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}