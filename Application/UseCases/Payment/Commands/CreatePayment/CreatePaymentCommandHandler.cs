using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UseCases.Payment.Commands.CreatePayment
{
    public class CreatePaymentCommandHandler : IRequestHandler<CreatePaymentCommand, int>
    {
        private readonly IApplicationDbContext _context;

        public CreatePaymentCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
        {
            var order = await _context.Orders
                .Include(o => o.Payment)
                .SingleOrDefaultAsync(o => o.Id.Equals(request.OrderId), cancellationToken);

            if (order is null)
            {
                throw new NotFoundException(nameof(Domain.Entities.Order), request.OrderId);
            }

            var entity = new Domain.Entities.Payment
            {
                Allowed = false,
                Order = order,
                OrderId = order.Id,
                Type = request.Type,
                CreditCardNumber = request.CreditCardNumber,
                CardExpMonth = request.CardExpMonth,
                CardExpYear = request.CardExpYear
            };

            await _context.Payments.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
    }
}