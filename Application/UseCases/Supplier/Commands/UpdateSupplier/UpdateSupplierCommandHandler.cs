using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UseCases.Supplier.Commands.UpdateSupplier
{
    public class UpdateSupplierCommandHandler : IRequestHandler<UpdateSupplierCommand>
    {
        private readonly IApplicationDbContext _context;

        public UpdateSupplierCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateSupplierCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Suppliers
                .SingleOrDefaultAsync(s => s.Id.Equals(request.Id), cancellationToken);

            if (entity is null)
            {
                throw new NotFoundException(nameof(Supplier), request.Id);
            }

            var checkForExistsEntity = _context.Categories
                .Any(category => category.Name.Equals(request.CompanyName));

            if (checkForExistsEntity)
            {
                throw new ItemExistsException($"{nameof(Domain.Entities.Supplier)} with that company name is already exists!");
            }

            entity.CompanyName = request.CompanyName;
            entity.Address = request.Address;
            entity.City = request.City;
            entity.Country = request.Country;
            entity.Email = request.Email;
            entity.PhoneNumber = request.PhoneNumber;

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}