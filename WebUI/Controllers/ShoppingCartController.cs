using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.UseCases.ShoppingCart.Commands.AddProduct;
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
                return View(await Mediator.Send(
                    new GetShoppingCartQuery { Username = User.Identity.Name }));
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
    }
}
