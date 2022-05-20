using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.UseCases.Customer.Queries.GetCustomer;
using Application.UseCases.Identity.User.Queries.GetUser;
using Application.UseCases.Order.Commands.CreateOrder;
using Application.UseCases.Order.Commands.DeleteOrder;
using Application.UseCases.Order.Queries.GetOrder;
using Application.UseCases.Order.Queries.GetOrdersForSpecifiedCustomer;
using Application.UseCases.Order.Queries.GetOrdersWithPagination;
using Application.UseCases.ShoppingCart.Queries.GetShoppingCart;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers
{
    public class OrderController : ApiControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] GetOrdersWithPaginationQuery query)
        {
            return View(await Mediator.Send(query));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            return View(await Mediator.Send(new GetOrderQuery { Id = id }));
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var user = await Mediator.Send(new GetUserQuery {UserName = User.Identity.Name});
            var shoppingCart = await Mediator.Send(new GetShoppingCartQuery {Id = user.ShoppingCart.Id});
            var customer = await Mediator.Send(new GetCustomerQuery {Id = user.Customer.Id});

            var id = await Mediator.Send(new CreateOrderCommand
            {
                Customer = customer,
                Products = shoppingCart.Products
            });

            return RedirectToAction("Create", "Delivery", new {orderId = id});
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id, string returnUrl = null)
        {
            try
            {
                await Mediator.Send(new DeleteOrderCommand { Id = id });

                if (returnUrl != null)
                {
                    return Redirect(returnUrl);
                }

                return RedirectToAction("Index", "Order");
            }
            catch (NotFoundException exception)
            {
                return View("Error", exception.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var user = await Mediator.Send(new GetUserQuery { UserName = User.Identity.Name });
            var customer = await Mediator.Send(new GetCustomerQuery {Id = user.Customer.Id});

            return View(await Mediator.Send(new GetOrdersForSpecifiedCustomerQuery {CustomerId = customer.Id}));
        }

        [HttpGet]
        public async Task<IActionResult> CancelCreation(int orderId)
        {
            await Mediator.Send(new DeleteOrderCommand {Id = orderId});

            return RedirectToAction("Index", "ShoppingCart");
        }
    }
}
