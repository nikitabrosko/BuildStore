using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;

namespace Application.UseCases.Delivery.Queries.GetDeliveriesWithPagination
{
    public class GetDeliveriesWithPaginationQueryHandler : IRequestHandler<GetDeliveriesWithPaginationQuery, PaginatedList<Domain.Entities.Delivery>>
    {
        private readonly IApplicationDbContext _context;

        public GetDeliveriesWithPaginationQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PaginatedList<Domain.Entities.Delivery>> Handle(GetDeliveriesWithPaginationQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Deliveries;

            return await PaginatedList<Domain.Entities.Delivery>.CreateAsync(query, request.PageNumber, request.PageSize);
        }
    }
}