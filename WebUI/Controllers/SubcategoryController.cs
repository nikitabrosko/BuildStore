using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.UseCases.Subcategory.Commands.CreateSubcategory;
using Application.UseCases.Subcategory.Queries.GetSubcategory;

namespace WebUI.Controllers
{
    public class SubcategoryController : ApiControllerBase
    {
        [HttpGet]
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

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Title = "Create Subcategory";

            return View();
        }

        [HttpPost]
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

            return RedirectToAction("GetCategory", "Category");
        }
    }
}