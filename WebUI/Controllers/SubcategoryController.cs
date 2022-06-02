using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.UseCases.Subcategory.Commands.AddSubcategory;
using Application.UseCases.Subcategory.Commands.CreateSubcategory;
using Application.UseCases.Subcategory.Commands.DeleteSubcategory;
using Application.UseCases.Subcategory.Commands.UpdateSubcategory;
using Application.UseCases.Subcategory.Queries.GetSubcategory;
using Microsoft.AspNetCore.Authorization;

namespace WebUI.Controllers
{
    public class SubcategoryController : ApiControllerBase
    {
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            try
            {
                var subcategory = await Mediator.Send(new GetSubcategoryQuery { Id = id });

                ViewBag.Title = subcategory.Name;

                return RedirectToAction("GetProducts", "Product",
                    new {subcategoryId = subcategory.Id});
            }
            catch (NotFoundException exception)
            {
                return View("Error", exception.Message);
            }
        }

        [Authorize(Roles = "admin")]
        [HttpGet("{id:int}")]
        public IActionResult Create([FromRoute] int id)
        {
            ViewBag.Title = "Create Subcategory";

            return View(new CreateSubcategoryCommand {CategoryId = id});
        }

        [Authorize(Roles = "admin")]
        [HttpPost("{command}")]
        public async Task<IActionResult> Create([FromForm] CreateSubcategoryCommand command)
        {
            try
            {
                await Mediator.Send(command);
            }
            catch (ItemExistsException exception)
            {
                return View("Error", exception.Message);
            }

            return RedirectToAction("Get", "Category", new {id = command.CategoryId});
        }

        [Authorize(Roles = "admin")]
        [HttpGet("{id:int}")]
        public IActionResult Add([FromRoute] int id)
        {
            ViewBag.Title = "Add Subcategory";

            return View(new AddSubcategoryCommand { SubcategoryId = id });
        }

        [Authorize(Roles = "admin")]
        [HttpPost("{command}")]
        public async Task<IActionResult> Add([FromForm] AddSubcategoryCommand command)
        {
            try
            {
                var categoryId = await Mediator.Send(command);

                return RedirectToAction("Get", "Category", new {id = categoryId});
            }
            catch (ItemExistsException exception)
            {
                return View("Error", exception.Message);
            }
        }

        [Authorize(Roles = "admin")]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id)
        {
            ViewBag.Title = "Update Subcategory";

            try
            {
                var entity = await Mediator.Send(new GetSubcategoryQuery {Id = id});

                var command = new UpdateSubcategoryCommand
                {
                    Id = id,
                    Name = entity.Name,
                    Description = entity.Description
                };

                return View(command);
            }
            catch (NotFoundException exception)
            {
                return View("Error", exception.Message);
            }
            catch (ItemExistsException exception)
            {
                return View("Error", exception.Message);
            }
        }

        [Authorize(Roles = "admin")]
        [HttpPost("{command}")]
        public async Task<IActionResult> Update([FromForm] UpdateSubcategoryCommand command)
        {
            try
            {
                var categoryId = await Mediator.Send(command);

                return RedirectToAction("Get", "Category", new { id = categoryId });
            }
            catch (NotFoundException exception)
            {
                return View("Error", exception.Message);
            }
            catch (ItemExistsException exception)
            {
                return View("Error", exception.Message);
            }
        }

        [Authorize(Roles = "admin")]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                ViewBag.Title = "Delete Category";

                var entity = await Mediator.Send(new GetSubcategoryQuery { Id = id });

                ViewBag.IsHaveSubcategories = entity.Subcategories.Count > 0;

                return View(new DeleteSubcategoryCommand { Id = id });
            }
            catch (NotFoundException exception)
            {
                return View("Error", exception.Message);
            }
        }

        [Authorize(Roles = "admin")]
        [HttpPost("{command}")]
        public async Task<IActionResult> Delete([FromForm] DeleteSubcategoryCommand command)
        {
            try
            {
                var categoryId = await Mediator.Send(command);

                return RedirectToAction("Get", "Category", new {id = categoryId});
            }
            catch (NotFoundException exception)
            {
                return View("Error", exception.Message);
            }
        }
    }
}