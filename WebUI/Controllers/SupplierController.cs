using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.UseCases.Supplier.Commands.CreateSupplier;
using Application.UseCases.Supplier.Commands.DeleteSupplier;
using Application.UseCases.Supplier.Commands.UpdateSupplier;
using Application.UseCases.Supplier.Queries.GetSupplier;
using Application.UseCases.Supplier.Queries.GetSuppliers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers
{
    [Authorize(Roles = "admin")]
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

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id)
        {
            ViewBag.Title = "Update Supplier";

            try
            {
                var entity = await Mediator.Send(new GetSupplierQuery { Id = id });
                
                var command = new UpdateSupplierCommand
                {
                    Id = id,
                    CompanyName = entity.CompanyName,
                    Address = entity.Address,
                    City = entity.City,
                    Country = entity.Country,
                    Email = entity.Email,
                    PhoneNumber = entity.PhoneNumber
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

        [HttpPost("{command}")]
        public async Task<IActionResult> Update([FromForm] UpdateSupplierCommand command)
        {
            try
            {
                await Mediator.Send(command);
            }
            catch (NotFoundException exception)
            {
                return View("Error", exception.Message);
            }
            catch (ItemExistsException exception)
            {
                return View("Error", exception.Message);
            }

            return RedirectToAction("Index");
        }

        [HttpGet("{id:int}")]
        public IActionResult Delete([FromRoute] int id)
        {
            ViewBag.Title = "Delete Supplier";

            return View(new DeleteSupplierCommand { Id = id });
        }

        [HttpPost("{command}")]
        public async Task<IActionResult> Delete([FromForm] DeleteSupplierCommand command)
        {
            try
            {
                await Mediator.Send(command);
            }
            catch (NotFoundException exception)
            {
                return View("Error", exception.Message);
            }

            return RedirectToAction("Index");
        }
    }
}
