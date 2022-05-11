using MediatR;

namespace Application.UseCases.Supplier.Commands.UpdateSupplier
{
    public class UpdateSupplierCommand : IRequest
    {
        public int Id { get; set; }

        public string CompanyName { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }
    }
}