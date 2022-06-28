using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Application.UseCases.Category.Commands.CreateCategory;
using Application.UseCases.Category.Commands.DeleteCategory;
using Application.UseCases.Category.Commands.UpdateCategory;
using Application.UseCases.Category.Queries.GetCategory;
using WebUI.Models.Category;
using Application.UseCases.Category.Queries.GetCategoriesWithPagination;
using Application.UseCases.Category.Queries.SearchCategoriesWithPagination;
using Application.UseCases.Category.Queries.GetCategories;
using Application.UseCases.Product.Queries.GetPaginatedProductsWithSubcategory;
using Application.UseCases.Product.Queries.SearchPaginatedProductsWithSubcategory;
using Application.UseCases.Product.Queries.GetProducts;
using Application.UseCases.ShoppingCart.Queries.GetShoppingCart;
using Application.UseCases.Identity.User.Queries.GetUser;
using Application.Common.Exceptions;
using Application.UseCases.Subcategory.Queries.GetSubcategory;
using Domain.Entities;
using Application.UseCases.Product.Queries.GetProduct;

namespace WebUI.Controllers
{
    public class CategoryController : ApiControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] GetCategoriesWithPaginationQuery query)
        {
            var categories = await Mediator.Send(query);
            var categoriesForHeader = await Mediator.Send(new GetCategoriesQuery());

            if (categories.Items.Count is 0 && categories.PageNumber > 1)
            {
                query.PageNumber -= 1;

                return View("Index", new ModelForCategories 
                { 
                    Categories = await Mediator.Send(query), 
                    CategoriesForHeader = categoriesForHeader 
                });
            }

            return View("Index", new ModelForCategories 
            { 
                Categories = categories, 
                CategoriesForHeader = categoriesForHeader 
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ModelForCreateCategory model)
        {
            await Mediator.Send(new CreateCategoryCommand
            {
                Name = model.Name,
                Description = model.Description,
                Picture = model.PictureRaw
            });

            return RedirectToAction("Index", "Category", 
                new GetCategoriesWithPaginationQuery 
                { 
                    PageNumber = model.PageNumber, 
                    PageSize = model.PageSize 
                });
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromForm] ModelForUpdateCategory model)
        {
            await Mediator.Send(new UpdateCategoryCommand 
            { 
                Id = model.Id, 
                Name = model.Name, 
                Description = model.Description, 
                Picture = model.PictureRaw 
            });

            return View("_CategoryPartial", new ModelForCategoryPartial 
            { 
                Category = await Mediator.Send(new GetCategoryQuery { Id = model.Id }), 
                ElementId = model.ElementId 
            });
        }

        [HttpPost("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            await Mediator.Send(new DeleteCategoryCommand 
            { 
                Id = id, 
                ProductsDeletion = false, 
                SubcategoriesDeletion = false 
            });

            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> Search([FromQuery] SearchCategoriesWithPaginationQuery query)
        {
            if (query.Pattern is null)
            {
                return RedirectToAction("Index", "Category");
            }

            var categories = await Mediator.Send(query);
            var categoriesForHeader = await Mediator.Send(new GetCategoriesQuery());

            if (categories.Items.Count is 0 && categories.PageNumber > 1)
            {
                query.PageNumber -= 1;

                return View("Index", new ModelForCategories 
                { 
                    Categories = await Mediator.Send(query), 
                    SearchPattern = query.Pattern, 
                    CategoriesForHeader = categoriesForHeader 
                });
            }

            return View("Index", new ModelForCategories 
            { 
                Categories = categories, 
                SearchPattern = query.Pattern, 
                CategoriesForHeader = categoriesForHeader 
            });
        }

        /*[HttpGet]
        public async Task<IActionResult> GetProducts([FromQuery] GetPaginatedProductsWithSubcategoryQuery query)
        {
            var products = await Mediator.Send(query);

            return View("_ShopProductsCategoryPartial", new ModelForShopProductsCategoryPartial 
            { 
                Products = products, 
                CategoryId = query.SubcategoryId
            });
        }

        [HttpGet]
        public async Task<IActionResult> SearchProducts([FromQuery] SearchPaginatedProductsWithSubcategoryQuery query)
        {
            var products = await Mediator.Send(query);

            return View("_ShopProductsCategoryPartial", new ModelForShopProductsCategoryPartial 
            { 
                Products = products, 
                CategoryId = query.SubcategoryId, 
                SearchPattern = query.Pattern
            });
        }

        [HttpPost]
        public async Task<IActionResult> SearchProductsForm([FromForm] SearchPaginatedProductsWithSubcategoryQuery query)
        {
            if (query.Pattern is null)
            {
                return RedirectToAction("Get", "Category", new GetPaginatedProductsWithSubcategoryQuery 
                { 
                    PageSize = query.PageSize, 
                    SubcategoryId = query.SubcategoryId 
                });
            }

            var products = await Mediator.Send(query);

            return View("_ShopProductsCategoryPartial", new ModelForShopProductsCategoryPartial 
            { 
                Products = products, 
                CategoryId = query.SubcategoryId, 
                SearchPattern = query.Pattern
            });
        }*/

        [HttpGet]
        public async Task<IActionResult> Shop([FromQuery] GetPaginatedProductsWithSubcategoryQuery query)
        {
            query.PageSize = 15;

            CategoryBase category;

            try
            {
                category = await Mediator.Send(new GetCategoryQuery { Id = query.SubcategoryId });
            }
            catch (NotFoundException)
            {
                category = await Mediator.Send(new GetSubcategoryQuery { Id = query.SubcategoryId });
            }

            if (User.IsInRole("admin"))
            {
                var modelForCategoryShop = new ModelForCategoryShop
                {
                    Categories = await Mediator.Send(new GetCategoriesQuery()),
                    ProductsPaginated = await Mediator.Send(query),
                    Products = await Mediator.Send(new GetProductsQuery()),
                    Category = category
                };

                return View("Shop", modelForCategoryShop);
            }
            else if (User.Identity.IsAuthenticated)
            {
                var user = await Mediator.Send(new GetUserQuery { UserName = User.Identity.Name });

                var modelForCategoryShop = new ModelForCategoryShop
                {
                    Categories = await Mediator.Send(new GetCategoriesQuery()),
                    ShoppingCart = await Mediator.Send(new GetShoppingCartQuery { Id = user.ShoppingCart.Id }),
                    ProductsPaginated = await Mediator.Send(query),
                    Products = await Mediator.Send(new GetProductsQuery()),
                    Category = category
                };

                return View("Shop", modelForCategoryShop);
            }
            else
            {
                var modelForCategoryShop = new ModelForCategoryShop
                {
                    Categories = await Mediator.Send(new GetCategoriesQuery()),
                    ProductsPaginated = await Mediator.Send(query),
                    Products = await Mediator.Send(new GetProductsQuery()),
                    Category = category
                };

                return View("Shop", modelForCategoryShop);
            }
        }

        [HttpGet]
        public async Task<IActionResult> ShopSearch([FromQuery] SearchPaginatedProductsWithSubcategoryQuery query)
        {
            if (query.Pattern is null)
            {
                return RedirectToAction("Shop", "Category");
            }

            CategoryBase category;

            try
            {
                category = await Mediator.Send(new GetCategoryQuery { Id = query.SubcategoryId });
            }
            catch (NotFoundException)
            {
                category = await Mediator.Send(new GetSubcategoryQuery { Id = query.SubcategoryId });
            }

            var products = await Mediator.Send(query);
            Domain.Entities.ShoppingCart shoppingCart = null;

            if (User.IsInRole("user"))
            {
                var user = await Mediator.Send(new GetUserQuery { UserName = User.Identity.Name });
                shoppingCart = await Mediator.Send(new GetShoppingCartQuery { Id = user.ShoppingCart.Id });
            }

            if (products.Items.Count is 0 && products.PageNumber > 1)
            {
                query.PageNumber -= 1;

                return View("Shop", new ModelForCategoryShop
                {
                    Categories = await Mediator.Send(new GetCategoriesQuery()),
                    SearchPattern = query.Pattern,
                    ProductsPaginated = await Mediator.Send(query),
                    Category = category,
                    Products = await Mediator.Send(new GetProductsQuery()),
                    ShoppingCart = shoppingCart
                });
            }

            return View("Shop", new ModelForCategoryShop
            {
                Categories = await Mediator.Send(new GetCategoriesQuery()),
                SearchPattern = query.Pattern,
                ProductsPaginated = products,
                Category = category,
                Products = await Mediator.Send(new GetProductsQuery()),
                ShoppingCart = shoppingCart
            });
        }

        [HttpPost]
        public IActionResult ShopSearchPost([FromForm] SearchPaginatedProductsWithSubcategoryQuery query)
        {
            return RedirectToAction("ShopSearch", "Category", query);
        }

        [HttpGet("{shoppingCartId:int}/{productId:int}")]
        public async Task<IActionResult> GetProductActions([FromRoute] int shoppingCartId, [FromRoute] int productId)
        {
            return View("_ProductActionsPartial", new ModelForProductActionsPartial 
            {
                ShoppingCart = await Mediator.Send(new GetShoppingCartQuery { Id = shoppingCartId }),
                Product = await Mediator.Send(new GetProductQuery { Id = productId })
            });
        }
    }
}