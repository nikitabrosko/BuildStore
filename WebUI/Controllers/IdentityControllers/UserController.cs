using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.UseCases.Identity.User.Commands.CreateUser;
using Application.UseCases.Identity.User.Commands.DeleteUser;
using Application.UseCases.Identity.User.Commands.UpdateUser;
using Application.UseCases.Identity.User.Queries.GetUsersWithPagination;
using Microsoft.AspNetCore.Authorization;

namespace WebUI.Controllers.IdentityControllers
{
    [Authorize(Roles = "admin")]
    public class UserController : ApiControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] GetUsersWithPaginationQuery query)
        {
            ViewBag.Title = "Users page";

            return View(await Mediator.Send(query));
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Title = "Create user page";

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateUserCommand command)
        {
            var result = await Mediator.Send(command);

            if (!result.Result.Succeeded)
            {
                return View("IdentityError", result.Result.Errors);
            }

            return RedirectToAction("Index");
        }

        [HttpDelete("{id}")]
        [Route("Delete/{id}")]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            try
            {
                await Mediator.Send(new DeleteUserCommand { Id = id });

                return RedirectToAction("Index");
            }
            catch (NotFoundException exception)
            {
                return View("Error", exception.Message);
            }
        }
    }
}
