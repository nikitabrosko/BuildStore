using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.UseCases.Identity.User.Queries.GetUser;
using Application.UseCases.ProductsDictionary.Commands.DeleteProductsDictionary;
using Application.UseCases.ShoppingCart.Commands.AddProduct;
using Application.UseCases.ShoppingCart.Commands.ClearProducts;
using Application.UseCases.ShoppingCart.Commands.RemoveProduct;
using Application.UseCases.ShoppingCart.Queries.GetShoppingCart;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace WebUI.Controllers
{
    [Authorize]
    public class ShoppingCartController : ApiControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var user = await Mediator.Send(new GetUserQuery { UserName = User.Identity.Name });

                ViewBag.User = user;

                return View(await Mediator.Send(
                    new GetShoppingCartQuery { Id = user.ShoppingCart.Id }));
            }
            catch (NotFoundException exception)
            {
                return View("Error", exception.Message);
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

        [HttpPost("{id:int}")]
        public async Task<IActionResult> AddProductPost([FromRoute] int id)
        {
            await Mediator.Send(new AddProductCommand
            {
                ProductId = id,
                Username = User.Identity.Name
            });

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

        [HttpPost("{id:int}")]
        public async Task<IActionResult> DeleteProductsDictionary([FromRoute] int id)
        {
            await Mediator.Send(new DeleteProductsDictionaryCommand { Id = id });

            var user = await Mediator.Send(new GetUserQuery { UserName = User.Identity.Name });
            var shoppingCart = await Mediator.Send(new GetShoppingCartQuery {Id = user.ShoppingCart.Id});

            return View("_ProductsPartial", shoppingCart);
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
    }
}
