using MediatR;

namespace Application.UseCases.Supplier.Commands.CreateSupplier
{
    public class CreateSupplierCommand : IRequest
    {
        public string CompanyName { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }
    }
}