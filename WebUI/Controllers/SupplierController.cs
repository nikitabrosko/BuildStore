using System.Threading.Tasks;
using Application.UseCases.Category.Queries.GetCategories;
using Application.UseCases.Supplier.Commands.CreateSupplier;
using Application.UseCases.Supplier.Commands.DeleteSupplier;
using Application.UseCases.Supplier.Commands.UpdateSupplier;
using Application.UseCases.Supplier.Queries.GetSupplier;
using Application.UseCases.Supplier.Queries.GetSuppliersWithPagination;
using Application.UseCases.Supplier.Queries.SearchSuppliersWithPagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebUI.Models.Supplier;

namespace WebUI.Controllers
{
    [Authorize(Roles = "admin")]
    public class SupplierController : ApiControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] GetSuppliersWithPaginationQuery query)
        {
            var suppliers = await Mediator.Send(query);
            var categoriesForHeader = await Mediator.Send(new GetCategoriesQuery());

            if (suppliers.Items.Count is 0 && suppliers.PageNumber > 1)
            {
                query.PageNumber -= 1;

                return View("Index", new ModelForSuppliers
                {
                    Suppliers = await Mediator.Send(query),
                    CategoriesForHeader = categoriesForHeader
                });
            }

            return View("Index", new ModelForSuppliers
            {
                Suppliers = suppliers,
                CategoriesForHeader = categoriesForHeader
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ModelForCreateSupplier model)
        {
            await Mediator.Send(new CreateSupplierCommand
            {
                CompanyName = model.CompanyName,
                Address = model.Address,
                City = model.City,
                Country = model.Country,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber
            });

            return RedirectToAction("Index", "Supplier",
                new GetSuppliersWithPaginationQuery
                {
                    PageNumber = model.PageNumber,
                    PageSize = model.PageSize
                });
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromForm] ModelForUpdateSupplier model)
        {
            await Mediator.Send(new UpdateSupplierCommand
            {
                Id = model.Id,
                CompanyName = model.CompanyName,
                Address = model.Address,
                City = model.City,
                Country = model.Country,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber
            });

            return View("_SupplierPartial", new ModelForSupplierPartial
            {
                Supplier = await Mediator.Send(new GetSupplierQuery { Id = model.Id }),
                ElementId = model.ElementId
            });
        }

        [HttpPost("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            await Mediator.Send(new DeleteSupplierCommand
            {
                Id = id,
                ProductsDeletion = false
            });

            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> Search([FromQuery] SearchSuppliersWithPaginationQuery query)
        {
            if (query.Pattern is null)
            {
                return RedirectToAction("Index", "Supplier");
            }

            var suppliers = await Mediator.Send(query);
            var categoriesForHeader = await Mediator.Send(new GetCategoriesQuery());

            if (suppliers.Items.Count is 0 && suppliers.PageNumber > 1)
            {
                query.PageNumber -= 1;

                return View("Index", new ModelForSuppliers
                {
                    Suppliers = await Mediator.Send(query),
                    SearchPattern = query.Pattern,
                    CategoriesForHeader = categoriesForHeader
                });
            }

            return View("Index", new ModelForSuppliers
            {
                Suppliers = suppliers,
                SearchPattern = query.Pattern,
                CategoriesForHeader = categoriesForHeader
            });
        }
    }
}
