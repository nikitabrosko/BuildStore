using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UseCases.Customer.Queries.GetCustomer
{
    public class GetCustomerQueryHandler : IRequestHandler<GetCustomerQuery, Domain.Entities.Customer>
    {
        private readonly IApplicationDbContext _context;
        private readonly IApplicationIdentityDbContext _identityContext;

        public GetCustomerQueryHandler(IApplicationDbContext context,
            IApplicationIdentityDbContext identityContext)
        {
            _context = context;
            _identityContext = identityContext;
        }

        public async Task<Domain.Entities.Customer> Handle(GetCustomerQuery request, CancellationToken cancellationToken)
        {
            var entity = (await _identityContext.Users
                .Include(u => u.Customer)
                .SingleOrDefaultAsync(u => u.UserName.Equals(request.UserName), cancellationToken)).Customer;

            if (entity is null)
            {
                throw new NotFoundException(nameof(Domain.Entities.Customer), request.UserName);
            }

            return entity;
        }
    }
}