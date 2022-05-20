using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UseCases.Payment.Commands.DeletePayment
{
    public class DeletePaymentCommandHandler : IRequestHandler<DeletePaymentCommand>
    {
        private readonly IApplicationDbContext _context;

        public DeletePaymentCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeletePaymentCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Payments
                .Include(p => p.Order)
                .ThenInclude(o => o.Payment)
                .SingleOrDefaultAsync(p => p.Id.Equals(request.Id), cancellationToken);

            if (entity is null)
            {
                throw new NotFoundException(nameof(Domain.Entities.Payment), request.Id);
            }

            entity.Order.Payment = null;
            entity.Order.PaymentId = default;

            _context.Payments.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}