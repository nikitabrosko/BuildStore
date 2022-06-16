using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Application.UseCases.Subcategory.Commands.CreateSubcategory;
using Application.UseCases.Subcategory.Commands.DeleteSubcategory;
using Application.UseCases.Subcategory.Commands.UpdateSubcategory;
using Application.UseCases.Subcategory.Queries.GetSubcategory;
using WebUI.Models.Subcategory;
using Application.UseCases.Subcategory.Queries.GetSubcategoriesWithPagination;
using Application.UseCases.Subcategory.Queries.SearchSubcategoriesWithPagination;
using Application.UseCases.Category.Queries.GetCategories;
using Application.UseCases.Subcategory.Commands.AddSubcategory;

namespace WebUI.Controllers
{
    public class SubcategoryController : ApiControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] GetSubcategoriesWithPaginationQuery query)
        {
            var subcategories = await Mediator.Send(query);
            var categoriesForHeader = await Mediator.Send(new GetCategoriesQuery());

            if (subcategories.Items.Count is 0 && subcategories.PageNumber > 1)
            {
                query.PageNumber -= 1;

                return View("Index", new ModelForSubcategories
                {
                    Subcategories = await Mediator.Send(query),
                    CategoriesForHeader = categoriesForHeader
                });
            }

            return View("Index", new ModelForSubcategories
            {
                Subcategories = subcategories,
                CategoriesForHeader = categoriesForHeader
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ModelForCreateSubcategory model)
        {
            await Mediator.Send(new CreateSubcategoryCommand
            {
                Name = model.Name,
                Description = model.Description,
                CategoryId = int.Parse(model.CategoryId)
            });

            return RedirectToAction("Index", "Subcategory",
                new GetSubcategoriesWithPaginationQuery
                {
                    PageNumber = model.PageNumber,
                    PageSize = model.PageSize
                });
        }

        [HttpPost]
        public async Task<IActionResult> AddSubcategory([FromForm] ModelForCreateSubcategory model)
        {
            await Mediator.Send(new AddSubcategoryCommand
            {
                Name = model.Name,
                Description = model.Description,
                SubcategoryId = int.Parse(model.CategoryId)
            });

            return RedirectToAction("Index", "Subcategory",
                new GetSubcategoriesWithPaginationQuery
                {
                    PageNumber = model.PageNumber,
                    PageSize = model.PageSize
                });
        }

        [HttpPost("{searchPattern}")]
        public async Task<IActionResult> AddSubcategory([FromForm] ModelForCreateSubcategory model, [FromRoute] string searchPattern = null)
        {
            await Mediator.Send(new AddSubcategoryCommand
            {
                Name = model.Name,
                Description = model.Description,
                SubcategoryId = int.Parse(model.CategoryId)
            });

            if (searchPattern is not null)
            {
                return RedirectToAction("Search", "Subcategory", new SearchSubcategoriesWithPaginationQuery
                {
                    PageNumber = model.PageNumber,
                    PageSize = model.PageSize,
                    Pattern = searchPattern
                });
            }

            return RedirectToAction("Index", "Subcategory",
                new GetSubcategoriesWithPaginationQuery
                {
                    PageNumber = model.PageNumber,
                    PageSize = model.PageSize
                });
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromForm] ModelForUpdateSubcategory model)
        {
            var command = new UpdateSubcategoryCommand
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description
            };

            if (model.CategoryId is not null)
            {
                command.NewCategoryId = int.Parse(model.CategoryId);
            }

            await Mediator.Send(command);

            return View("_SubcategoryPartial", new ModelForSubcategoryPartial
            {
                Subcategory = await Mediator.Send(new GetSubcategoryQuery { Id = model.Id }),
                ElementId = model.ElementId
            });
        }

        [HttpPost("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            await Mediator.Send(new DeleteSubcategoryCommand
            {
                Id = id,
                ProductsDeletion = false,
                SubcategoriesDeletion = false
            });

            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> Search([FromQuery] SearchSubcategoriesWithPaginationQuery query)
        {
            if (query.Pattern is null)
            {
                return RedirectToAction("Index", "Subcategory");
            }

            var subcategories = await Mediator.Send(query);
            var categoriesForHeader = await Mediator.Send(new GetCategoriesQuery());

            if (subcategories.Items.Count is 0 && subcategories.PageNumber > 1)
            {
                query.PageNumber -= 1;

                return View("Index", new ModelForSubcategories
                {
                    Subcategories = await Mediator.Send(query),
                    SearchPattern = query.Pattern,
                    CategoriesForHeader = categoriesForHeader
                });
            }

            return View("Index", new ModelForSubcategories
            {
                Subcategories = subcategories,
                SearchPattern = query.Pattern,
                CategoriesForHeader = categoriesForHeader
            });
        }
    }
}