using MediatR;

namespace Application.UseCases.Customer.Commands.UpdateCustomer
{
    public class UpdateCustomerCommand : IRequest<int>
    {
        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public string Phone { get; set; }
    }
}