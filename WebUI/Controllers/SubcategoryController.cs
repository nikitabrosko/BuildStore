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
        public async Task<IActionResult> Get([FromRoute] int id)
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

            return RedirectToAction("Get", "Category", new {id = command.CategoryId});
        }

        [HttpGet("{id:int}")]
        public IActionResult Add([FromRoute] int id)
        {
            ViewBag.Title = "Add Subcategory";

            return View(new AddSubcategoryCommand { SubcategoryId = id });
        }

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
    }
}