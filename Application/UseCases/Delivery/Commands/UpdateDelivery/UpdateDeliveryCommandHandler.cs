using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UseCases.Delivery.Commands.UpdateDelivery
{
    public class UpdateDeliveryCommandHandler : IRequestHandler<UpdateDeliveryCommand>
    {
        private readonly IApplicationDbContext _context;

        public UpdateDeliveryCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateDeliveryCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Deliveries
                .SingleOrDefaultAsync(d => d.Id.Equals(request.Id), cancellationToken);

            if (entity is null)
            {
                throw new NotFoundException(nameof(Domain.Entities.Delivery), request.Id);
            }

            entity.Fulfilled = request.Fulfilled;
            entity.Type = request.Type;

            _context.Deliveries.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}