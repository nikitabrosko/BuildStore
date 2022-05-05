using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.UseCases.Category.Commands.CreateCategory;
using Application.UseCases.Category.Commands.UpdateCategory;
using Application.UseCases.Category.Queries.GetCategories;
using Application.UseCases.Category.Queries.GetCategory;

namespace WebUI.Controllers
{
    public class CategoryController : ApiControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] GetCategoriesQuery query)
        {
            ViewBag.Title = "Categories";

            return View(await Mediator.Send(query));
        }

        [Route("CategoryGetCategory/{id:int}")]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            try
            {
                var category = await Mediator.Send(new GetCategoryQuery { Id = id });

                ViewBag.Title = category.Name;

                return View(category);
            }
            catch (NotFoundException exception)
            {
                return View("Error", exception.Message);
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Title = "Create Category";

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateCategoryCommand command)
        {
            try
            {
                await Mediator.Send(command);
            }
            catch (ItemExistsException exception)
            {
                return View("Error", exception.Message);
            }

            return RedirectToAction("Index");
        }

        [HttpGet("{id:int}")]
        public IActionResult Update([FromRoute] int id)
        {
            ViewBag.Title = "Update Category";

            return View(new UpdateCategoryCommand {Id = id});
        }

        [HttpPost("{command}")]
        [Route("Update/{command}")]
        public async Task<IActionResult> Update([FromForm] UpdateCategoryCommand command)
        {
            try
            {
                await Mediator.Send(command);
            }
            catch (ItemExistsException exception)
            {
                return View("Error", exception.Message);
            }

            return RedirectToAction("Index");
        }
    }
}
