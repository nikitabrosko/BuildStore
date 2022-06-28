using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.UseCases.Category.Queries.GetCategories;
using Application.UseCases.Identity.User.Queries.GetUser;
using Application.UseCases.Product.Commands.CreateProduct;
using Application.UseCases.Product.Commands.DeleteProduct;
using Application.UseCases.Product.Commands.UpdateProduct;
using Application.UseCases.Product.Queries.GetProduct;
using Application.UseCases.Product.Queries.GetProducts;
using Application.UseCases.Product.Queries.GetProductsWithPagination;
using Application.UseCases.Product.Queries.GetRelatedProducts;
using Application.UseCases.Product.Queries.GetSupplierProducts;
using Application.UseCases.Product.Queries.SearchProductsWithPagination;
using Application.UseCases.ShoppingCart.Queries.GetShoppingCart;
using Application.UseCases.Supplier.Queries.GetSuppliers;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using WebUI.Models.Product;

namespace WebUI.Controllers
{
    public class ProductController : ApiControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] GetProductsWithPaginationQuery query)
        {
            var products = await Mediator.Send(query);
            var suppliers = await Mediator.Send(new GetSuppliersQuery());
            var categoriesForHeader = await Mediator.Send(new GetCategoriesQuery());

            if (products.Items.Count is 0 && products.PageNumber > 1)
            {
                query.PageNumber -= 1;

                return View("Index", new ModelForProducts
                {
                    Products = await Mediator.Send(query),
                    CategoriesForHeader = categoriesForHeader,
                    Suppliers = suppliers
                });
            }

            return View("Index", new ModelForProducts
            {
                Products = products,
                CategoriesForHeader = categoriesForHeader,
                Suppliers = suppliers
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ModelForCreateProduct model)
        {
            await Mediator.Send(new CreateProductCommand
            {
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                Weight = model.Weight,
                QuantityPerUnit = model.QuantityPerUnit,
                Discount = model.Discount,
                CategoryId = int.Parse(model.CategoryId),
                SupplierId = int.Parse(model.SupplierId),
                Pictures = model.Pictures
            });

            return RedirectToAction("Index", "Product",
                new GetProductsWithPaginationQuery
                {
                    PageNumber = model.PageNumber,
                    PageSize = model.PageSize
                });
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromForm] ModelForUpdateProduct model)
        {
            var command = new UpdateProductCommand
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                Weight = model.Weight,
                QuantityPerUnit = model.QuantityPerUnit,
                Discount = model.Discount,
                Pictures = model.Pictures
            };

            if (model.CategoryId is not null)
            {
                command.CategoryId = int.Parse(model.CategoryId);
            }

            if (model.SupplierId is not null)
            {
                command.SupplierId = int.Parse(model.SupplierId);
            }

            await Mediator.Send(command);

            return View("_ProductPartial", new ModelForProductPartial
            {
                Product = await Mediator.Send(new GetProductQuery { Id = model.Id }),
                ElementId = model.ElementId
            });
        }

        [HttpPost("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            await Mediator.Send(new DeleteProductCommand
            {
                Id = id
            });

            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> Search([FromQuery] SearchProductsWithPaginationQuery query)
        {
            if (query.Pattern is null)
            {
                return RedirectToAction("Index", "Product");
            }

            var products = await Mediator.Send(query);
            var suppliers = await Mediator.Send(new GetSuppliersQuery());
            var categoriesForHeader = await Mediator.Send(new GetCategoriesQuery());

            if (products.Items.Count is 0 && products.PageNumber > 1)
            {
                query.PageNumber -= 1;

                return View("Index", new ModelForProducts
                {
                    Products = await Mediator.Send(query),
                    SearchPattern = query.Pattern,
                    CategoriesForHeader = categoriesForHeader,
                    Suppliers = suppliers
                });
            }

            return View("Index", new ModelForProducts
            {
                Products = products,
                SearchPattern = query.Pattern,
                CategoriesForHeader = categoriesForHeader,
                Suppliers = suppliers
            });
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Details([FromRoute] int id)
        {
            var product = await Mediator.Send(new GetProductQuery { Id = id });

            var relatedProducts = new List<Product>();

            if (product.Supplier is not null)
            {
                relatedProducts = (await Mediator.Send(new GetSupplierProductsQuery { Supplier = product.Supplier })).ToList();
            }

            if (User.IsInRole("admin"))
            {
                var modelForProductDetails = new ModelForProductDetails
                {
                    Categories = await Mediator.Send(new GetCategoriesQuery()),
                    Product = product,
                    RelatedProducts = relatedProducts,
                    Products = await Mediator.Send(new GetProductsQuery())
                };

                return View("ProductDetails", modelForProductDetails);
            }
            else if (User.Identity.IsAuthenticated)
            {
                var user = await Mediator.Send(new GetUserQuery { UserName = User.Identity.Name });

                var modelForProductDetails = new ModelForProductDetails
                {
                    Categories = await Mediator.Send(new GetCategoriesQuery()),
                    ShoppingCart = await Mediator.Send(new GetShoppingCartQuery { Id = user.ShoppingCart.Id }),
                    Product = product,
                    RelatedProducts = relatedProducts,
                    Products = await Mediator.Send(new GetProductsQuery())
                };

                return View("ProductDetails", modelForProductDetails);
            }
            else
            {
                var modelForProductDetails = new ModelForProductDetails
                {
                    Categories = await Mediator.Send(new GetCategoriesQuery()),
                    Product = product,
                    RelatedProducts = relatedProducts,
                    Products = await Mediator.Send(new GetProductsQuery())
                };

                return View("ProductDetails", modelForProductDetails);
            }
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