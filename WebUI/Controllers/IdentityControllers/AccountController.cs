using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.UseCases.Identity.User.Commands.CreateUser;
using Application.UseCases.Identity.User.Commands.UpdateUser;
using Application.UseCases.Identity.User.Queries.GetUser;
using Application.UseCases.Identity.User.Queries.LoginUser;
using Domain.IdentityEntities;
using Microsoft.AspNetCore.Identity;
using WebUI.Models.Account;
using Application.UseCases.Category.Queries.GetCategories;
using Application.UseCases.ShoppingCart.Queries.GetShoppingCart;
using Application.UseCases.Identity.User.Queries.CheckUserNameForExists;
using Application.UseCases.Identity.User.Queries.CheckPassword;

namespace WebUI.Controllers.IdentityControllers
{
    public class AccountController : ApiControllerBase
    {
        private readonly SignInManager<User> _signInManager;

        public AccountController(SignInManager<User> signInManager)
        {
            _signInManager = signInManager;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromForm] CreateUserCommand command)
        {
            var result = await Mediator.Send(command);

            if (result.Result.Succeeded)
            {
                await _signInManager.SignInAsync(result.User, false);

                return RedirectToAction("Index", "Home");
            }

            return View("Index", "Account");
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("admin"))
            {
                var modelForLogin = new ModelForLogin
                {
                    Categories = await Mediator.Send(new GetCategoriesQuery())
                };

                return View("Index", modelForLogin);
            }
            else if (User.Identity.IsAuthenticated)
            {
                var user = await Mediator.Send(new GetUserQuery { UserName = User.Identity.Name });

                var modelForLogin = new ModelForLogin
                {
                    Categories = await Mediator.Send(new GetCategoriesQuery()),
                    ShoppingCart = await Mediator.Send(new GetShoppingCartQuery { Id = user.ShoppingCart.Id })
                };

                return View("Index", modelForLogin);
            }
            else
            {
                var modelForLogin = new ModelForLogin
                {
                    Categories = await Mediator.Send(new GetCategoriesQuery())
                };

                return View("Index", modelForLogin);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromForm] LoginUserQuery query)
        {
            var result = await Mediator.Send(query);

            if (result.Succeeded)
            {
                if (query.ReturnUrl is not null)
                {
                    return Redirect(query.ReturnUrl);
                }

                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Index", "Account");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> MyAccount()
        {
            var user = await Mediator.Send(new GetUserQuery { UserName = User.Identity.Name });

            var modelForMyAccount = new ModelForMyAccount
            {
                Categories = await Mediator.Send(new GetCategoriesQuery()),
                ShoppingCart = await Mediator.Send(new GetShoppingCartQuery { Id = user.ShoppingCart.Id }),
                User = user
            };

            return View("MyAccount", modelForMyAccount);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword([FromForm] string password)
        {
            var user = await Mediator.Send(new GetUserQuery {UserName = User.Identity.Name});

            await Mediator.Send(new UpdateUserCommand { Id = user.Id, Password = password });

            return RedirectToAction("Logout");
        }

        [HttpPost]
        public async Task<IActionResult> ChangeUserName([FromForm] string userName)
        {
            var user = await Mediator.Send(new GetUserQuery { UserName = User.Identity.Name });

            await Mediator.Send(new UpdateUserCommand { Id = user.Id, Name = userName });

            return RedirectToAction("Logout");
        }

        [HttpPost]
        public async Task<IActionResult> ChangeEmail([FromForm] string email)
        {
            var user = await Mediator.Send(new GetUserQuery { UserName = User.Identity.Name });

            await Mediator.Send(new UpdateUserCommand { Id = user.Id, Email = email });

            return RedirectToAction("MyAccount");
        }

        [HttpGet("{username}")]
        public async Task<bool> CheckUserName([FromRoute] string username)
        {
            return await Mediator.Send(new CheckUserNameForExistsQuery { UserName = username });
        }

        [HttpGet]
        public async Task<bool> CheckPassword([FromQuery] string username, [FromQuery] string password)
        {
            return await Mediator.Send(new CheckPasswordQuery { User = await Mediator.Send(new GetUserQuery { UserName = username }), Password = password });
        }
    }
}
