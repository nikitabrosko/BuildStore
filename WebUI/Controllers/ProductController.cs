using System;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.UseCases.Product.Commands.CreateProduct;
using Application.UseCases.Product.Commands.DeleteProduct;
using Application.UseCases.Product.Commands.UpdateProduct;
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

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id)
        {
            ViewBag.Title = "Update Product";

            try
            {
                var entity = await Mediator.Send(new GetProductQuery { Id = id });
                var imgSrc = $"data:image/gif;base64,{Convert.ToBase64String(entity.Picture)}";

                ViewBag.Picture = imgSrc;

                var command = new UpdateProductCommand
                {
                    Id = id,
                    Name = entity.Name,
                    Description = entity.Description,
                    Price = entity.Price,
                    Discount = entity.Discount,
                    QuantityPerUnit = entity.QuantityPerUnit,
                    Weight = entity.Weight
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
        public async Task<IActionResult> Update([FromForm] UpdateProductCommand command)
        {
            try
            {
                var supplierId = await Mediator.Send(command);

                return RedirectToAction("Get", "Supplier", new { id = supplierId });
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

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                var supplierId = await Mediator.Send(new DeleteProductCommand {Id = id});

                return RedirectToAction("Get", "Supplier", new { id = supplierId });
            }
            catch (NotFoundException exception)
            {
                return View("Error", exception.Message);
            }
        }
    }
}
