using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UseCases.Order.Queries.GetOrder
{
    public class GetOrderQueryHandler : IRequestHandler<GetOrderQuery, Domain.Entities.Order>
    {
        private readonly IApplicationDbContext _context;

        public GetOrderQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Domain.Entities.Order> Handle(GetOrderQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context.Orders
                .Include(o => o.Delivery)
                .Include(o => o.Payment)
                .Include(o => o.ProductsDictionary)
                .ThenInclude(p => p.Product)
                .SingleOrDefaultAsync(o => o.Id.Equals(request.Id), cancellationToken);

            if (entity is null)
            {
                throw new NotFoundException(nameof(Domain.Entities.Order), request.Id);
            }

            return entity;
        }
    }
}