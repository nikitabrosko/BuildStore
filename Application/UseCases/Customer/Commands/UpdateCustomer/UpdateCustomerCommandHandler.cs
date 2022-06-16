using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UseCases.Customer.Commands.UpdateCustomer
{
    public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, int>
    {
        private readonly IApplicationDbContext _context;
        private readonly IApplicationIdentityDbContext _identityContext;

        public UpdateCustomerCommandHandler(IApplicationDbContext context,
            IApplicationIdentityDbContext identityContext)
        {
            _context = context;
            _identityContext = identityContext;
        }

        public async Task<int> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {
            var entity = (await _identityContext.Users
                .Include(u => u.Customer)
                .SingleOrDefaultAsync(u => u.UserName.Equals(request.UserName), cancellationToken)).Customer;
            
            entity.FirstName = request.FirstName;
            entity.LastName = request.LastName;
            entity.Address = request.Address;
            entity.City = request.City;
            entity.Country = request.Country;
            entity.Phone = request.Phone;

            _context.Customers.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
            await _identityContext.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
    }
}