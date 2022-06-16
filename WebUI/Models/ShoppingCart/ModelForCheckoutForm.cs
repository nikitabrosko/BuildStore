using Application.UseCases.Customer.Commands.CreateCustomer;
using Application.UseCases.Order.Commands.CreateOrder;

namespace WebUI.Models.ShoppingCart
{
    public class ModelForCheckoutForm
    {
        public CreateCustomerCommand Customer { get; set; }

        public CreateOrderCommand Order { get; set; }

        public Domain.Entities.ShoppingCart ShoppingCart { get; set; }

        public string EmailMessage { get; set; }

        public string EmailAddress { get; set; }
    }
}
