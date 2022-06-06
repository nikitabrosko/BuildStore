using System;
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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers
{
    [Authorize]
    public class OrderController : ApiControllerBase
    {
        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] GetOrdersWithPaginationQuery query)
        {
            return View(await Mediator.Send(query));
        }

        [Authorize(Roles = "admin")]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                return View(await Mediator.Send(new GetOrderQuery { Id = id }));
            }
            catch (NotFoundException exception)
            {
                return View("Error", exception.Message);
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            try
            {
                return View("Create");
            }
            catch (NotFoundException exception)
            {
                return View("Error", exception.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateOrderCommand command)
        {
            try
            {

                var user = await Mediator.Send(new GetUserQuery { UserName = User.Identity.Name });
                var shoppingCart = await Mediator.Send(new GetShoppingCartQuery { Id = user.ShoppingCart.Id });
                var customer = await Mediator.Send(new GetCustomerQuery { Id = user.Customer.Id });

                command.Customer = customer;
                command.ProductsDictionary = shoppingCart.ProductsDictionary;

                await Mediator.Send(command);

                return RedirectToAction("Clear", "ShoppingCart");
            }
            catch (NotFoundException exception)
            {
                return View("Error", exception.Message);
            }
        }

        [Authorize(Roles = "admin")]
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
            try
            {
                var user = await Mediator.Send(new GetUserQuery { UserName = User.Identity.Name });
                var customer = await Mediator.Send(new GetCustomerQuery { Id = user.Customer.Id });

                return View(await Mediator.Send(new GetOrdersForSpecifiedCustomerQuery { CustomerId = customer.Id }));
            }
            catch (NotFoundException exception)
            {
                return View("Error", exception.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetOrderProducts(int orderId)
        {
            try
            {
                var order = await Mediator.Send(new GetOrderQuery { Id = orderId });

                return View("OrderProducts", order.ProductsDictionary);
            }
            catch (NotFoundException exception)
            {
                return View("Error", exception.Message);
            }
        }
    }
}
