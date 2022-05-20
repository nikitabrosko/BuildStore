using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UseCases.Delivery.Queries.GetDelivery
{
    public class GetDeliveryQueryHandler : IRequestHandler<GetDeliveryQuery, Domain.Entities.Delivery>
    {
        private readonly IApplicationDbContext _context;

        public GetDeliveryQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Domain.Entities.Delivery> Handle(GetDeliveryQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context.Deliveries
                .Include(d => d.Order)
                .SingleOrDefaultAsync(d => d.Id.Equals(request.Id), cancellationToken);

            if (entity is null)
            {
                throw new NotFoundException(nameof(Domain.Entities.Delivery), request.Id);
            }

            return entity;
        }
    }
}