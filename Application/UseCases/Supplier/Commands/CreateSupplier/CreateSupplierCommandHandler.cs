using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using MediatR;

namespace Application.UseCases.Supplier.Commands.CreateSupplier
{
    public class CreateSupplierCommandHandler : IRequestHandler<CreateSupplierCommand>
    {
        private readonly IApplicationDbContext _context;

        public CreateSupplierCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(CreateSupplierCommand request, CancellationToken cancellationToken)
        {
            var entity = new Domain.Entities.Supplier
            {
                CompanyName = request.CompanyName,
                Address = request.Address,
                City = request.City,
                Country = request.Country,
                PhoneNumber = request.PhoneNumber,
                Email = request.Email
            };

            var checkForExistsEntity = _context.Suppliers
                .Any(s => s.CompanyName.Equals(entity.CompanyName));

            if (checkForExistsEntity)
            {
                throw new ItemExistsException($"{nameof(Domain.Entities.Supplier)} with this company name is already exists!");
            }

            await _context.Suppliers.AddAsync(entity, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}