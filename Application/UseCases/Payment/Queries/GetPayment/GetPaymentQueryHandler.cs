using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UseCases.Payment.Queries.GetPayment
{
    public class GetPaymentQueryHandler : IRequestHandler<GetPaymentQuery, Domain.Entities.Payment>
    {
        private readonly IApplicationDbContext _context;

        public GetPaymentQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Domain.Entities.Payment> Handle(GetPaymentQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context.Payments
                .Include(p => p.Order)
                .SingleOrDefaultAsync(p => p.Id.Equals(request.Id), cancellationToken);

            if (entity is null)
            {
                throw new NotFoundException(nameof(Domain.Entities.Payment), request.Id);
            }

            return entity;
        }
    }
}