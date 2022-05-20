using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.UseCases.Identity.User.Queries.GetUser;
using Application.UseCases.ShoppingCart.Commands.AddProduct;
using Application.UseCases.ShoppingCart.Commands.ClearProducts;
using Application.UseCases.ShoppingCart.Commands.RemoveProduct;
using Application.UseCases.ShoppingCart.Queries.GetShoppingCart;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> AddProduct([FromRoute] int id, string returnUrl = null)
        {
            await Mediator.Send(new AddProductCommand
            {
                ProductId = id,
                Username = User.Identity.Name
            });

            if (returnUrl != null)
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "ShoppingCart");
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> RemoveProduct([FromRoute] int id)
        {
            await Mediator.Send(new RemoveProductCommand
            {
                ProductId = id,
                Username = User.Identity.Name
            });

            return RedirectToAction("Index", "ShoppingCart");
        }

        [HttpGet]
        public async Task<IActionResult> Clear()
        {
            var user = await Mediator.Send(new GetUserQuery {UserName = User.Identity.Name});
            var shoppingCartId = user.ShoppingCart.Id;

            await Mediator.Send(new ClearProductsCommand {Id = shoppingCartId});

            return RedirectToAction("Index", "ShoppingCart");
        }
    }
}
