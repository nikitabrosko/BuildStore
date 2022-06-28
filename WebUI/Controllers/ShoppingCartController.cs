using System;
using System.IO;
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
using Application.UseCases.Order.Queries.GetOrder;
using Application.UseCases.ProductsDictionary.Commands.DeleteProductsDictionary;
using Application.UseCases.ProductsDictionary.Queries.GetProductsDictionary;
using Application.UseCases.ShoppingCart.Commands.AddProduct;
using Application.UseCases.ShoppingCart.Commands.ClearProducts;
using Application.UseCases.ShoppingCart.Commands.RemoveProduct;
using Application.UseCases.ShoppingCart.Queries.GetShoppingCart;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
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

            var order = await Mediator.Send(new GetOrderQuery { Id = orderId });

            await Mediator.Send(new ClearProductsCommand { Id = shoppingCart.Id });
            await SendEmailAsync(orderId, model);

            var pdfDoc = GeneratePdfFile(model, order);
            var fileCollection = new FormFileCollection();

            await SendEmailToUserAsync(pdfDoc);

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

        private async Task SendEmailToUserAsync(byte[] pdfDoc)
        {
            await Mediator.Send(new SendEmailCommand
            {
                Subject = $"Ваш заказ на сайте BuildStore",
                EmailAddress = (await Mediator.Send(new GetUserQuery { UserName = User.Identity.Name })).Email,
                Content = "Ваш заказ в данном прикреплённом pdf-файле!",
                Attachment = pdfDoc
            });
        }

        private byte[] GeneratePdfFile(ModelForCheckoutForm model, Order order)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            var document = new PdfDocument();

            var page = document.AddPage();

            var gfx = XGraphics.FromPdfPage(page);

            var font = new XFont("Arial", 15);
            var fontForProducts = new XFont("Arial", 12);

            gfx.DrawString($"Товарный чек", new XFont("Arial", 21, XFontStyle.Bold), XBrushes.Black, new XPoint(230, 30));
            gfx.DrawString($"Имя пользователя: {User.Identity.Name}", font, XBrushes.Black, new XPoint(30, 70));
            gfx.DrawString($"Фамилия и имя заказчика: {model.Customer.LastName} {model.Customer.FirstName}", font, XBrushes.Black, new XPoint(30, 100));
            gfx.DrawString($"Адрес доставки: {model.Customer.Country}, {model.Customer.City}, {model.Customer.Address}", font, XBrushes.Black, new XPoint(30, 130));
            gfx.DrawString($"Тип доставки: {model.Order.DeliveryType}", font, XBrushes.Black, new XPoint(30, 160));
            gfx.DrawString($"Способ оплаты: {order.Payment.Type}", font, XBrushes.Black, new XPoint(30, 190));
            gfx.DrawString($"Общая сумма заказа: BYN {order.ProductsDictionary.Sum(p => p.Product.Price * p.Count)}", new XFont("Arial", 15, XFontStyle.Bold), XBrushes.Red, new XPoint(30, 220));

            gfx.DrawString($"Товары", font, XBrushes.Black, new XPoint(250, 250));

            var yPoint = 280;
            var counter = 1;

            var firstProducts = order.ProductsDictionary.Take(5);

            foreach (var productsDictionary in firstProducts)
            {
                var container = gfx.BeginContainer();
                gfx.DrawString($"Товар №{counter}", fontForProducts, XBrushes.Black, new XPoint(30, yPoint));
                gfx.DrawString($"Название: {productsDictionary.Product.Name}", fontForProducts, XBrushes.Black, new XPoint(30, yPoint + 20));
                gfx.DrawString($"Стоимость: {productsDictionary.Product.Price}", fontForProducts, XBrushes.Black, new XPoint(30, yPoint + 40));
                gfx.DrawString($"Количество: {productsDictionary.Count}", fontForProducts, XBrushes.Black, new XPoint(30, yPoint + 60));
                gfx.DrawString($"Сумма: {productsDictionary.Product.Price * productsDictionary.Count}", fontForProducts, XBrushes.Black, new XPoint(30, yPoint + 80));
                gfx.EndContainer(container);

                yPoint += 110;
                counter++;
            }

            gfx.Dispose();

            if (order.ProductsDictionary.Count > 5)
            {
                for (int i = 0; i < Math.Ceiling((order.ProductsDictionary.Count - (decimal)5) / 7); i++)
                {
                    var newPage = document.AddPage();

                    var newGfx = XGraphics.FromPdfPage(newPage);

                    yPoint = 30;

                    foreach (var productsDictionary in order.ProductsDictionary.Skip(5 + (i * 7)).Take(7))
                    {
                        var container = newGfx.BeginContainer();
                        newGfx.DrawString($"Товар №{counter}", fontForProducts, XBrushes.Black, new XPoint(30, yPoint));
                        newGfx.DrawString($"Название: {productsDictionary.Product.Name}", fontForProducts, XBrushes.Black, new XPoint(30, yPoint + 20));
                        newGfx.DrawString($"Стоимость: {productsDictionary.Product.Price}", fontForProducts, XBrushes.Black, new XPoint(30, yPoint + 40));
                        newGfx.DrawString($"Количество: {productsDictionary.Count}", fontForProducts, XBrushes.Black, new XPoint(30, yPoint + 60));
                        newGfx.DrawString($"Сумма: {productsDictionary.Product.Price * productsDictionary.Count}", fontForProducts, XBrushes.Black, new XPoint(30, yPoint + 80));
                        newGfx.EndContainer(container);

                        yPoint += 110;
                        counter++;
                    }

                    newGfx.Dispose();
                }
            }

            byte[] fileBytes;

            using (var stream = new MemoryStream())
            {
                document.Save(stream, false);
                fileBytes = stream.ToArray();
            }

            return fileBytes;
        }
    }
}