using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;

namespace Application.UseCases.Payment.Queries.GetPaymentsWithPagination
{
    public class GetPaymentsWithPaginationQueryHandler : IRequestHandler<GetPaymentsWithPaginationQuery, PaginatedList<Domain.Entities.Payment>>
    {
        private readonly IApplicationDbContext _context;

        public GetPaymentsWithPaginationQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PaginatedList<Domain.Entities.Payment>> Handle(GetPaymentsWithPaginationQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Payments;

            return await PaginatedList<Domain.Entities.Payment>.CreateAsync(query, request.PageNumber, request.PageSize);
        }
    }
}