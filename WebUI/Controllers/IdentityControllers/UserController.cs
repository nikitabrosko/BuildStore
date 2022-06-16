using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Application.UseCases.Identity.User.Commands.CreateUser;
using Application.UseCases.Identity.User.Commands.DeleteUser;
using Application.UseCases.Identity.User.Commands.UpdateUser;
using Application.UseCases.Identity.User.Queries.GetUsersWithPagination;
using Microsoft.AspNetCore.Authorization;
using Application.UseCases.Category.Queries.GetCategories;
using WebUI.Models.User;
using Application.UseCases.Identity.User.Queries.GetUserViaId;
using Application.UseCases.Identity.User.Queries.SearchUsersWithPagination;
using Application.UseCases.Identity.User.Queries.GetUsersWithRolesWithPagination;
using Application.UseCases.Identity.User.Queries.SearchUsersWithRolesWithPagination;
using Application.UseCases.Identity.Role.Commands.AddRoleToUser;
using System.Linq;
using Application.UseCases.Identity.Role.Queries.GetRole;
using Application.UseCases.Identity.Role.Commands.RemoveRoleFromUser;
using Microsoft.AspNetCore.Identity;

namespace WebUI.Controllers.IdentityControllers
{
    [Authorize(Roles = "admin")]
    public class UserController : ApiControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] GetUsersWithRolesWithPaginationQuery query)
        {
            var users = await Mediator.Send(query);
            var roles = _roleManager.Roles;
            var categoriesForHeader = await Mediator.Send(new GetCategoriesQuery());

            if (users.Items.Count is 0 && users.PageNumber > 1)
            {
                query.PageNumber -= 1;

                return View("Index", new ModelForUsers
                {
                    Users = await Mediator.Send(query),
                    CategoriesForHeader = categoriesForHeader,
                    Roles = roles
                });
            }

            return View("Index", new ModelForUsers
            {
                Users = users,
                CategoriesForHeader = categoriesForHeader,
                Roles = roles
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ModelForCreateUser model)
        {
            await Mediator.Send(new CreateUserCommand
            {
                Name = model.Name,
                Email = model.Email,
                Password = model.Password
            });

            return RedirectToAction("Index", "User",
                new GetUsersWithRolesWithPaginationQuery
                {
                    PageNumber = model.PageNumber,
                    PageSize = model.PageSize
                });
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromForm] ModelForUpdateUser model)
        {
            var command = new UpdateUserCommand
            {
                Id = model.Id,
                Name = model.Name,
                Email = model.Email,
                Password = model.Password
            };

            await Mediator.Send(command);

            return View("_UserPartial", new ModelForUserPartial
            {
                User = await Mediator.Send(new GetUserViaIdQuery { Id = model.Id }),
                ElementId = model.ElementId
            });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRoles([FromForm] ModelForUpdateUserRoles model)
        {
            var user = await Mediator.Send(new GetUserViaIdQuery { Id = model.UserId });

            foreach (var role in model.Roles)
            {
                if (!user.Roles.Contains(role))
                {
                    await Mediator.Send(new AddRoleToUserCommand 
                    { 
                        UserId = user.User.Id, 
                        RoleId = (await Mediator.Send(new GetRoleQuery { Name = role })).Id
                    });
                }
            }

            foreach (var role in user.Roles)
            {
                if (!model.Roles.Contains(role))
                {
                    await Mediator.Send(new RemoveRoleFromUserCommand
                    {
                        UserId = user.User.Id,
                        RoleName = role
                    });
                }
            }

            return View("_UserPartial", new ModelForUserPartial
            {
                User = await Mediator.Send(new GetUserViaIdQuery { Id = user.User.Id }),
                ElementId = model.ElementId
            });
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            await Mediator.Send(new DeleteUserCommand
            {
                Id = id
            });

            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> Search([FromQuery] SearchUsersWithRolesWithPaginationQuery query)
        {
            if (query.Pattern is null)
            {
                return RedirectToAction("Index", "User");
            }

            var users = await Mediator.Send(query);
            var roles = _roleManager.Roles;
            var categoriesForHeader = await Mediator.Send(new GetCategoriesQuery());

            if (users.Items.Count is 0 && users.PageNumber > 1)
            {
                query.PageNumber -= 1;

                return View("Index", new ModelForUsers
                {
                    Users = await Mediator.Send(query),
                    SearchPattern = query.Pattern,
                    CategoriesForHeader = categoriesForHeader,
                    Roles = roles
                });
            }

            return View("Index", new ModelForUsers
            {
                Users = users,
                SearchPattern = query.Pattern,
                CategoriesForHeader = categoriesForHeader,
                Roles = roles
            });
        }
    }
}
