using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.UseCases.Supplier.Commands.CreateSupplier;
using Application.UseCases.Supplier.Queries.GetSupplier;
using Application.UseCases.Supplier.Queries.GetSuppliers;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers
{
    public class SupplierController : ApiControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] GetSuppliersQuery query)
        {
            ViewBag.Title = "Suppliers";

            return View(await Mediator.Send(query));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            try
            {
                var supplier = await Mediator.Send(new GetSupplierQuery { Id = id });

                ViewBag.Title = supplier.CompanyName;

                return View(supplier);
            }
            catch (NotFoundException exception)
            {
                return View("Error", exception.Message);
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Title = "Create Supplier";

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateSupplierCommand command)
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
