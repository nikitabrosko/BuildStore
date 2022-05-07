using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.UseCases.Product.Commands.CreateProduct;
using Application.UseCases.Product.Queries.GetProduct;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers
{
    public class ProductController : ApiControllerBase
    {
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var product = await Mediator.Send(new GetProductQuery { Id = id });

                ViewBag.Title = product.Name;

                return View(product);
            }
            catch (NotFoundException exception)
            {
                return View("Error", exception.Message);
            }
        }

        [HttpGet("{id:int}")]
        public IActionResult Create([FromRoute] int id)
        {
            ViewBag.Title = "Create Product";

            return View(new CreateProductCommand { SupplierId = id });
        }

        [HttpPost("{command}")]
        public async Task<IActionResult> Create([FromForm] CreateProductCommand command)
        {
            try
            {
                await Mediator.Send(command);
            }
            catch (ItemExistsException exception)
            {
                return View("Error", exception.Message);
            }

            return RedirectToAction("Get", "Supplier", new { id = command.SupplierId });
        }
    }
}
