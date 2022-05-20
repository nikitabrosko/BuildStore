using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UseCases.Delivery.Commands.DeleteDelivery
{
    public class DeleteDeliveryCommandHandler : IRequestHandler<DeleteDeliveryCommand>
    {
        private readonly IApplicationDbContext _context;

        public DeleteDeliveryCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteDeliveryCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Deliveries
                .Include(d => d.Order)
                .ThenInclude(o => o.Delivery)
                .SingleOrDefaultAsync(d => d.Id.Equals(request.Id), cancellationToken);

            if (entity is null)
            {
                throw new NotFoundException(nameof(Domain.Entities.Delivery), request.Id);
            }

            entity.Order.Delivery = null;
            entity.Order.DeliveryId = default;

            _context.Deliveries.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}