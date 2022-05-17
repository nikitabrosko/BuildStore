using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.IdentityEntities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UseCases.Customer.Commands.CreateCustomer
{
    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IApplicationIdentityDbContext _identityContext;

        public CreateCustomerCommandHandler(IApplicationDbContext context,
            IApplicationIdentityDbContext identityContext)
        {
            _context = context;
            _identityContext = identityContext;
        }

        public async Task<Unit> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            var userEntity = await _identityContext.Users
                .Include(u => u.Customer)
                .SingleOrDefaultAsync(u => u.UserName.Equals(request.UserName), cancellationToken);

            if (userEntity is null)
            {
                throw new NotFoundException(nameof(User), request.UserName);
            }

            var customerEntity = new Domain.Entities.Customer
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Address = request.Address,
                City = request.City,
                Country = request.Country,
                Phone = request.Phone,
                CreditCardNumber = request.CreditCardNumber,
                CardExpMonth = request.CardExpMonth,
                CardExpYear = request.CardExpYear
            };

            await _context.Customers.AddAsync(customerEntity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            userEntity.Customer = customerEntity;

            _identityContext.Users.Update(userEntity);
            await _identityContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}