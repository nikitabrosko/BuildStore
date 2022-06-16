using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Application.UseCases.Identity.Role.Commands.CreateRole;
using Application.UseCases.Identity.Role.Commands.DeleteRole;
using Application.UseCases.Identity.Role.Commands.UpdateRole;
using Application.UseCases.Identity.Role.Queries.GetRolesWithPagination;
using Microsoft.AspNetCore.Authorization;
using Application.UseCases.Category.Queries.GetCategories;
using WebUI.Models.Role;
using Application.UseCases.Identity.Role.Queries.GetRole;
using Application.UseCases.Identity.Role.Queries.SearchRolesWithPagination;

namespace WebUI.Controllers.IdentityControllers
{
    [Authorize(Roles = "admin")]
    public class RoleController : ApiControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] GetRolesWithPaginationQuery query)
        {
            var roles = await Mediator.Send(query);
            var categoriesForHeader = await Mediator.Send(new GetCategoriesQuery());

            if (roles.Items.Count is 0 && roles.PageNumber > 1)
            {
                query.PageNumber -= 1;

                return View("Index", new ModelForRoles
                {
                    Roles = await Mediator.Send(query),
                    CategoriesForHeader = categoriesForHeader,
                });
            }

            return View("Index", new ModelForRoles
            {
                Roles = roles,
                CategoriesForHeader = categoriesForHeader
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ModelForCreateRole model)
        {
            await Mediator.Send(new CreateRoleCommand
            {
                Name = model.Name
            });

            return RedirectToAction("Index", "Role",
                new GetRolesWithPaginationQuery
                {
                    PageNumber = model.PageNumber,
                    PageSize = model.PageSize
                });
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromForm] ModelForUpdateRole model)
        {
            var command = new UpdateRoleCommand
            {
                Id = model.Id,
                Name = model.Name
            };

            await Mediator.Send(command);

            return View("_RolePartial", new ModelForRolePartial
            {
                Role = await Mediator.Send(new GetRoleQuery { Id = model.Id }),
                ElementId = model.ElementId
            });
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            await Mediator.Send(new DeleteRoleCommand
            {
                Id = id
            });

            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> Search([FromQuery] SearchRolesWithPaginationQuery query)
        {
            if (query.Pattern is null)
            {
                return RedirectToAction("Index", "Role");
            }

            var roles = await Mediator.Send(query);
            var categoriesForHeader = await Mediator.Send(new GetCategoriesQuery());

            if (roles.Items.Count is 0 && roles.PageNumber > 1)
            {
                query.PageNumber -= 1;

                return View("Index", new ModelForRoles
                {
                    Roles = await Mediator.Send(query),
                    SearchPattern = query.Pattern,
                    CategoriesForHeader = categoriesForHeader
                });
            }

            return View("Index", new ModelForRoles
            {
                Roles = roles,
                SearchPattern = query.Pattern,
                CategoriesForHeader = categoriesForHeader
            });
        }
    }
}
