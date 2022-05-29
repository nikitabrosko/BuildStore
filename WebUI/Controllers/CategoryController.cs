using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.UseCases.Category.Commands.CreateCategory;
using Application.UseCases.Category.Commands.DeleteCategory;
using Application.UseCases.Category.Commands.UpdateCategory;
using Application.UseCases.Category.Queries.GetCategories;
using Application.UseCases.Category.Queries.GetCategory;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;

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

        [Authorize(Roles = "admin")]
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Title = "Create Category";

            return View();
        }

        [Authorize(Roles = "admin")]
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

        [Authorize(Roles = "admin")]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id)
        {
            ViewBag.Title = "Update Category";

            try
            {
                var entity = await Mediator.Send(new GetCategoryQuery {Id = id});
                var imgSrc = $"data:image/gif;base64,{Convert.ToBase64String(entity.Picture)}";

                ViewBag.Picture = imgSrc;

                var command = new UpdateCategoryCommand
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
        public async Task<IActionResult> Update([FromForm] UpdateCategoryCommand command)
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

        [Authorize(Roles = "admin")]
        [HttpGet("{id:int}")]
        public IActionResult Delete([FromRoute] int id)
        {
            ViewBag.Title = "Delete Category";

            return View(new DeleteCategoryCommand {Id = id});
        }

        [Authorize(Roles = "admin")]
        [HttpPost("{command}")]
        public async Task<IActionResult> Delete([FromForm] DeleteCategoryCommand command)
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

        [HttpPost]
        public async Task<IActionResult> GetCategories()
        {
            return View("_DropdownCategoriesPartial", await Mediator.Send(new GetCategoriesQuery()));
        }
    }
}
