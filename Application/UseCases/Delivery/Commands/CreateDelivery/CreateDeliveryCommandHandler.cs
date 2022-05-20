using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UseCases.Delivery.Commands.CreateDelivery
{
    public class CreateDeliveryCommandHandler : IRequestHandler<CreateDeliveryCommand, int>
    {
        private readonly IApplicationDbContext _context;

        public CreateDeliveryCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateDeliveryCommand request, CancellationToken cancellationToken)
        {
            var order = await _context.Orders
                .Include(o => o.Delivery)
                .SingleOrDefaultAsync(o => o.Id.Equals(request.OrderId), cancellationToken);

            if (order is null)
            {
                throw new NotFoundException(nameof(Domain.Entities.Order), request.OrderId);
            }

            var entity = new Domain.Entities.Delivery
            {
                Order = order,
                OrderId = order.Id,
                Type = request.Type,
                Fulfilled = false
            };

            await _context.Deliveries.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
    }
}