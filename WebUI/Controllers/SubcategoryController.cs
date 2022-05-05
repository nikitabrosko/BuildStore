using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.UseCases.Subcategory.Commands.AddSubcategory;
using Application.UseCases.Subcategory.Commands.CreateSubcategory;
using Application.UseCases.Subcategory.Queries.GetSubcategory;

namespace WebUI.Controllers
{
    public class SubcategoryController : ApiControllerBase
    {
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetSubcategory([FromRoute] int id)
        {
            try
            {
                var subcategory = await Mediator.Send(new GetSubcategoryQuery { Id = id });

                ViewBag.Title = subcategory.Name;

                return View(subcategory);
            }
            catch (NotFoundException exception)
            {
                return View("Error", exception.Message);
            }
        }

        [HttpGet("{id:int}")]
        public IActionResult Create([FromRoute] int id)
        {
            ViewBag.Title = "Create Subcategory";

            return View(new CreateSubcategoryCommand {CategoryId = id});
        }

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

            return RedirectToAction("GetCategory", "Category", new {id = command.CategoryId});
        }

        [HttpGet("{id:int}")]
        public IActionResult AddSubcategory([FromRoute] int id)
        {
            ViewBag.Title = "Add Subcategory";

            return View(new AddSubcategoryCommand { SubcategoryId = id });
        }

        [HttpPost("{command}")]
        public async Task<IActionResult> AddSubcategory([FromForm] AddSubcategoryCommand command)
        {
            try
            {
                var categoryId = await Mediator.Send(command);

                return RedirectToAction("GetCategory", "Category", new {id = categoryId});
            }
            catch (ItemExistsException exception)
            {
                return View("Error", exception.Message);
            }
        }
    }
}