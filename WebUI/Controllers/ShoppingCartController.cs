using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.UseCases.Category.Queries.GetCategories;
using Application.UseCases.Customer.Commands.CreateCustomer;
using Application.UseCases.Customer.Commands.UpdateCustomer;
using Application.UseCases.Customer.Queries.GetCustomer;
using Application.UseCases.Email.Commands.SendEmail;
using Application.UseCases.Identity.User.Queries.GetUser;
using Application.UseCases.Order.Commands.CreateOrder;
using Application.UseCases.ProductsDictionary.Commands.DeleteProductsDictionary;
using Application.UseCases.ProductsDictionary.Queries.GetProductsDictionary;
using Application.UseCases.ShoppingCart.Commands.AddProduct;
using Application.UseCases.ShoppingCart.Commands.ClearProducts;
using Application.UseCases.ShoppingCart.Commands.RemoveProduct;
using Application.UseCases.ShoppingCart.Queries.GetShoppingCart;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebUI.Models;
using WebUI.Models.ShoppingCart;

namespace WebUI.Controllers
{
    [Authorize]
    public class ShoppingCartController : ApiControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("admin"))
            {
                var modelForShoppingCart = new ModelForShoppingCart
                {
                    Categories = await Mediator.Send(new GetCategoriesQuery())
                };

                return View("Index", modelForShoppingCart);
            }
            else if (User.Identity.IsAuthenticated)
            {
                var user = await Mediator.Send(new GetUserQuery { UserName = User.Identity.Name });

                var modelForShoppingCart = new ModelForShoppingCart
                {
                    Categories = await Mediator.Send(new GetCategoriesQuery()),
                    ShoppingCart = await Mediator.Send(new GetShoppingCartQuery { Id = user.ShoppingCart.Id })
                };

                return View("Index", modelForShoppingCart);
            }
            else
            {
                var modelForShoppingCart = new ModelForShoppingCart
                {
                    Categories = await Mediator.Send(new GetCategoriesQuery()),
                    ShoppingCart = null
                };

                return View("Index", modelForShoppingCart);
            }
        }

        [HttpGet("{id:int}")]
        public async Task AddProductGet([FromRoute] int id)
        {
            await Mediator.Send(new AddProductCommand
            {
                ProductId = id,
                Username = User.Identity.Name
            });
        }

        [HttpPost("{id:int}/{amount:int}")]
        public async Task<IActionResult> AddProductPost([FromRoute] int id, [FromRoute] int amount = 1)
        {
            for (var i = 0; i < amount; i++)
            {
                await Mediator.Send(new AddProductCommand
                {
                    ProductId = id,
                    Username = User.Identity.Name
                });
            }

            var user = await Mediator.Send(new GetUserQuery { UserName = User.Identity.Name });
            var shoppingCart = await Mediator.Send(new GetShoppingCartQuery { Id = user.ShoppingCart.Id });

            return View("_ProductsPartial", shoppingCart);
        }

        [HttpPost("{id:int}")]
        public async Task<IActionResult> RemoveProduct([FromRoute] int id)
        {
            await Mediator.Send(new RemoveProductCommand
            {
                ProductId = id,
                Username = User.Identity.Name
            });

            var user = await Mediator.Send(new GetUserQuery { UserName = User.Identity.Name });
            var shoppingCart = await Mediator.Send(new GetShoppingCartQuery { Id = user.ShoppingCart.Id });

            return View("_ProductsPartial", shoppingCart);
        }

        [HttpGet]
        public async Task<IActionResult> Clear()
        {
            try
            {
                var user = await Mediator.Send(new GetUserQuery { UserName = User.Identity.Name });
                var shoppingCartId = user.ShoppingCart.Id;

                await Mediator.Send(new ClearProductsCommand { Id = shoppingCartId });

                return RedirectToAction("Index", "ShoppingCart");
            }
            catch (NotFoundException exception)
            {
                return View("Error", exception.Message);
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> DeleteProductsDictionary([FromRoute] int id)
        {
            await Mediator.Send(new DeleteProductsDictionaryCommand { Id = id });

            var user = await Mediator.Send(new GetUserQuery { UserName = User.Identity.Name });

            return View("_ShoppingCartPartial", await Mediator.Send(new GetShoppingCartQuery { Id = user.ShoppingCart.Id }));
        }

        [Authorize(Roles = "user")]
        [HttpGet]
        public async Task<IActionResult> GetShoppingCartWithProductsCount()
        {
            var user = await Mediator.Send(new GetUserQuery { UserName = User.Identity.Name });
            var shoppingCart = await Mediator.Send(new GetShoppingCartQuery { Id = user.ShoppingCart.Id });
            var productsCount = shoppingCart.ProductsDictionary.Sum(productsDictionary => productsDictionary.Count);

            ViewBag.ProductsCount = productsCount;
            return View("_ShoppingCartPartial", productsCount);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> AddProductToShoppingCart([FromRoute] int id)
        {
            await Mediator.Send(new AddProductCommand
            {
                ProductId = id,
                Username = User.Identity.Name
            });

            var user = await Mediator.Send(new GetUserQuery { UserName = User.Identity.Name });

            return View("_ShoppingCartPartial", await Mediator.Send(new GetShoppingCartQuery { Id = user.ShoppingCart.Id }));
        }

        [HttpPost]
        public async Task<IActionResult> AddProductToShoppingCart([FromForm] int id, [FromForm] int amount = 1)
        {
            await Mediator.Send(new AddProductCommand
            {
                ProductId = id,
                Username = User.Identity.Name,
                Amount = amount
            });

            var user = await Mediator.Send(new GetUserQuery { UserName = User.Identity.Name });

            return View("_ShoppingCartPartial", await Mediator.Send(new GetShoppingCartQuery { Id = user.ShoppingCart.Id }));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> RemoveProductFromShoppingCart([FromRoute] int id)
        {
            await Mediator.Send(new RemoveProductCommand
            {
                ProductId = id,
                Username = User.Identity.Name
            });

            var user = await Mediator.Send(new GetUserQuery { UserName = User.Identity.Name });

            return View("_ShoppingCartPartial", await Mediator.Send(new GetShoppingCartQuery { Id = user.ShoppingCart.Id }));
        }

        [HttpGet("{productsDictionaryId:int}/{elementId}")]
        public async Task<IActionResult> GetProduct([FromRoute] int productsDictionaryId, [FromRoute] string elementId)
        {
            var entity = await Mediator.Send(new GetProductsDictionaryQuery { Id = productsDictionaryId });

            if (entity is not null)
            {
                return View("_ProductPartial", new ModelForProductPartial
                {
                    ProductsDictionary = entity,
                    ElementId = elementId
                });
            }

            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> Checkout()
        {
            if (User.IsInRole("admin"))
            {
                var modelForCheckout = new ModelForCheckout
                {
                    Categories = await Mediator.Send(new GetCategoriesQuery())
                };

                return View("Checkout", modelForCheckout);
            }
            else if (User.Identity.IsAuthenticated)
            {
                var user = await Mediator.Send(new GetUserQuery { UserName = User.Identity.Name });
                var shoppingCart = await Mediator.Send(new GetShoppingCartQuery { Id = user.ShoppingCart.Id });
                var customer = new CreateCustomerCommand();

                if (user.Customer is not null)
                {
                    var customerEntity = await Mediator.Send(new GetCustomerQuery { Id = user.Customer.Id });
                    customer.FirstName = customerEntity.FirstName;
                    customer.LastName = customerEntity.LastName;
                    customer.Address = customerEntity.Address;
                    customer.City = customerEntity.City;
                    customer.Country = customerEntity.Country;
                    customer.Phone = customerEntity.Phone;
                }

                var modelForCheckout = new ModelForCheckout
                {
                    Categories = await Mediator.Send(new GetCategoriesQuery()),
                    ShoppingCart = shoppingCart,
                    ModelForForm = new ModelForCheckoutForm 
                    { 
                        EmailAddress = user.Email, 
                        Customer = customer, 
                        Order = new CreateOrderCommand(),
                        ShoppingCart = shoppingCart
                    }
                };

                return View("Checkout", modelForCheckout);
            }
            else
            {
                var modelForCheckout = new ModelForCheckout
                {
                    Categories = await Mediator.Send(new GetCategoriesQuery())
                };

                return View("Checkout", modelForCheckout);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Checkout([FromForm] ModelForCheckoutForm model)
        {
            var user = await Mediator.Send(new GetUserQuery { UserName = User.Identity.Name });
            var shoppingCart = await Mediator.Send(new GetShoppingCartQuery { Id = user.ShoppingCart.Id });

            int customerId;

            if (user.Customer is null)
            {
                customerId = await Mediator.Send(model.Customer);
            }
            else
            {
                customerId = await Mediator.Send(new UpdateCustomerCommand
                {
                    UserName = user.UserName,
                    FirstName = model.Customer.FirstName,
                    LastName = model.Customer.LastName,
                    Address = model.Customer.Address,
                    City = model.Customer.City,
                    Country = model.Customer.Country,
                    Phone = model.Customer.Phone
                });
            }

            model.Order.ProductsDictionary = shoppingCart.ProductsDictionary;
            model.Order.Customer = await Mediator.Send(new GetCustomerQuery { Id = customerId });
            var orderId = await Mediator.Send(model.Order);

            await Mediator.Send(new ClearProductsCommand { Id = shoppingCart.Id });
            await SendEmailAsync(orderId, model);

            return RedirectToAction("Index", "Home");
        }

        private async Task SendEmailAsync(int orderId, ModelForCheckoutForm model)
        {
            await Mediator.Send(new SendEmailCommand
            {
                Subject = $"Заказ №{orderId}",
                EmailAddress = "buildstoreshop@gmail.com",
                Content =
                $"<p>Имя пользователя: {model.Customer.UserName}</p>" +
                $"<p>Фамилия и имя: {model.Customer.LastName} {model.Customer.FirstName}</p>" +
                $"<p>Адрес доставки: {model.Customer.Address}</p>" +
                $"<p>Город доставки: {model.Customer.City}</p>" +
                $"<p>Страна доставки: {model.Customer.Country}</p>" +
                $"<p>Номер телефона: {model.Customer.Phone}</p>" +
                $"<p>Тип доставки: {model.Order.DeliveryType}</p>" +
                $"<p>Тип платежа: {model.Order.PaymentType}</p>" +
                $"<p>Дополнительная информация: {model.EmailMessage}</p>"
            });
        }
    }
}