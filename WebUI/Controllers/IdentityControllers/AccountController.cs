using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.UseCases.Identity.User.Commands.CreateUser;
using Application.UseCases.Identity.User.Commands.UpdateUser;
using Application.UseCases.Identity.User.Queries.GetUser;
using Application.UseCases.Identity.User.Queries.LoginUser;
using Domain.IdentityEntities;
using Microsoft.AspNetCore.Identity;

namespace WebUI.Controllers.IdentityControllers
{
    public class AccountController : ApiControllerBase
    {
        private readonly SignInManager<User> _signInManager;

        public AccountController(SignInManager<User> signInManager)
        {
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Register(string returnUrl = null)
        {
            ViewBag.Title = "Register page";

            return View(new CreateUserCommand {ReturnUrl = returnUrl});
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromForm] CreateUserCommand command)
        {
            var result = await Mediator.Send(command);

            if (result.Result.Succeeded)
            {
                await _signInManager.SignInAsync(result.User, false);

                if (command.ReturnUrl != null)
                {
                    return Redirect(command.ReturnUrl);
                }

                return RedirectToAction("Index", "Home");
            }

            return View("IdentityError", result.Result.Errors);
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewBag.Title = "Login page";

            return View("_SignInPartial", new LoginUserQuery { ReturnUrl = returnUrl });
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromForm] LoginUserQuery query)
        {
            try
            {
                var result = await Mediator.Send(query);

                if (result.Succeeded)
                {
                    if (query.ReturnUrl != null)
                    {
                        return Redirect(query.ReturnUrl);
                    }

                    return RedirectToAction("Index", "Home");
                }

                return View("Error", "Something went wrong. Check password!");
            }
            catch (NotFoundException exception)
            {
                return View("Error", exception.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Cabinet()
        {
            try
            {
                return View(await Mediator.Send(new GetUserQuery { UserName = User.Identity.Name }));
            }
            catch (NotFoundException exception)
            {
                return View("Error", exception.Message);
            }
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword([FromForm] UpdateUserCommand command)
        {
            var user = await Mediator.Send(new GetUserQuery {UserName = User.Identity.Name});
            command.Id = user.Id;

            await Mediator.Send(command);

            return RedirectToAction("Cabinet", "Account");
        }

        [HttpGet]
        public async Task<IActionResult> ChangeUserName()
        {
            var user = await Mediator.Send(new GetUserQuery { UserName = User.Identity.Name });

            return View(new UpdateUserCommand {Id = user.Id, Name = user.UserName});
        }

        [HttpPost]
        public async Task<IActionResult> ChangeUserName([FromForm] UpdateUserCommand command)
        {
            await Mediator.Send(command);

            return RedirectToAction("Cabinet", "Account");
        }

        [HttpGet]
        public async Task<IActionResult> ChangeEmail()
        {
            var user = await Mediator.Send(new GetUserQuery { UserName = User.Identity.Name });

            return View(new UpdateUserCommand { Id = user.Id, Email = user.Email });
        }

        [HttpPost]
        public async Task<IActionResult> ChangeEmail([FromForm] UpdateUserCommand command)
        {
            await Mediator.Send(command);

            return RedirectToAction("Cabinet", "Account");
        }
    }
}
