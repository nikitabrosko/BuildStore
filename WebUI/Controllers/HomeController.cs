using Application.UseCases.Category.Queries.GetCategories;
using Application.UseCases.Identity.User.Queries.GetUser;
using Application.UseCases.Product.Queries.GetPaginatedProductsWithSubcategory;
using Application.UseCases.Product.Queries.GetProduct;
using Application.UseCases.Product.Queries.GetProducts;
using Application.UseCases.Product.Queries.GetProductsWithPagination;
using Application.UseCases.Product.Queries.SearchProductsWithPagination;
using Application.UseCases.ShoppingCart.Queries.GetShoppingCart;
using Domain.IdentityEntities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using WebUI.Models.Home;

namespace WebUI.Controllers
{
    public class HomeController : Controller
    {
        private ISender _mediator;

        protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();

        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("admin"))
            {
                var modelForIndex = new ModelForIndex
                {
                    Categories = await Mediator.Send(new GetCategoriesQuery()),
                    Products = await Mediator.Send(new GetProductsQuery { Count = 20 })
                };

                return View("Index", modelForIndex);
            }
            else if (User.Identity.IsAuthenticated)
            {
                var user = await Mediator.Send(new GetUserQuery { UserName = User.Identity.Name });

                var modelForIndex = new ModelForIndex
                {
                    Categories = await Mediator.Send(new GetCategoriesQuery()),
                    ShoppingCart = await Mediator.Send(new GetShoppingCartQuery { Id = user.ShoppingCart.Id }),
                    Products = await Mediator.Send(new GetProductsQuery { Count = 20 })
                };

                return View("Index", modelForIndex);
            }
            else
            {
                var modelForIndex = new ModelForIndex
                {
                    Categories = await Mediator.Send(new GetCategoriesQuery()),
                    ShoppingCart = null,
                    Products = await Mediator.Send(new GetProductsQuery { Count = 20 })
                };

                return View("Index", modelForIndex);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Contacts()
        {
            if (User.IsInRole("admin"))
            {
                var modelForContacts = new ModelForContacts
                {
                    Categories = await Mediator.Send(new GetCategoriesQuery())
                };

                return View("Contacts", modelForContacts);
            }
            else if (User.Identity.IsAuthenticated)
            {
                var user = await Mediator.Send(new GetUserQuery { UserName = User.Identity.Name });

                var modelForContacts = new ModelForContacts
                {
                    Categories = await Mediator.Send(new GetCategoriesQuery()),
                    ShoppingCart = await Mediator.Send(new GetShoppingCartQuery { Id = user.ShoppingCart.Id })
                };

                return View("Contacts", modelForContacts);
            }
            else
            {
                var modelForContacts = new ModelForContacts
                {
                    Categories = await Mediator.Send(new GetCategoriesQuery())
                };

                return View("Contacts", modelForContacts);
            }
        }

        [HttpGet]
        public async Task<IActionResult> About()
        {
            if (User.IsInRole("admin"))
            {
                var modelForAbout = new ModelForAbout
                {
                    Categories = await Mediator.Send(new GetCategoriesQuery())
                };

                return View("About", modelForAbout);
            }
            else if (User.Identity.IsAuthenticated)
            {
                var user = await Mediator.Send(new GetUserQuery { UserName = User.Identity.Name });

                var modelForAbout = new ModelForAbout
                {
                    Categories = await Mediator.Send(new GetCategoriesQuery()),
                    ShoppingCart = await Mediator.Send(new GetShoppingCartQuery { Id = user.ShoppingCart.Id })
                };

                return View("About", modelForAbout);
            }
            else
            {
                var modelForAbout = new ModelForAbout
                {
                    Categories = await Mediator.Send(new GetCategoriesQuery())
                };

                return View("About", modelForAbout);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Shop([FromQuery] GetProductsWithPaginationQuery query)
        {
            query.PageSize = 15;

            if (User.IsInRole("admin"))
            {
                var modelForShop = new ModelForShop
                {
                    Categories = await Mediator.Send(new GetCategoriesQuery()),
                    ProductsPaginated = await Mediator.Send(query),
                    Products = await Mediator.Send(new GetProductsQuery())
                };

                return View("Shop", modelForShop);
            }
            else if (User.Identity.IsAuthenticated)
            {
                var user = await Mediator.Send(new GetUserQuery { UserName = User.Identity.Name });

                var modelForShop = new ModelForShop
                {
                    Categories = await Mediator.Send(new GetCategoriesQuery()),
                    ShoppingCart = await Mediator.Send(new GetShoppingCartQuery { Id = user.ShoppingCart.Id }),
                    ProductsPaginated = await Mediator.Send(query),
                    Products = await Mediator.Send(new GetProductsQuery())
                };

                return View("Shop", modelForShop);
            }
            else
            {
                var modelForShop = new ModelForShop
                {
                    Categories = await Mediator.Send(new GetCategoriesQuery()),
                    ProductsPaginated = await Mediator.Send(query),
                    Products = await Mediator.Send(new GetProductsQuery())
                };

                return View("Shop", modelForShop);
            }
        }

        [HttpGet]
        public async Task<IActionResult> ShopSearch([FromQuery] SearchProductsWithPaginationQuery query)
        {
            query.PageSize = 15;

            if (query.Pattern is null)
            {
                return RedirectToAction("Shop", "Home");
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

                return View("Shop", new ModelForShop
                {
                    Categories = await Mediator.Send(new GetCategoriesQuery()),
                    SearchPattern = query.Pattern,
                    ProductsPaginated = await Mediator.Send(query),
                    Products = await Mediator.Send(new GetProductsQuery()),
                    ShoppingCart = shoppingCart
                });
            }

            return View("Shop", new ModelForShop
            {
                Categories = await Mediator.Send(new GetCategoriesQuery()),
                SearchPattern = query.Pattern,
                ProductsPaginated = products,
                Products = await Mediator.Send(new GetProductsQuery()),
                ShoppingCart = shoppingCart
            });
        }

        [HttpPost]
        public IActionResult ShopSearchPost([FromForm] SearchProductsWithPaginationQuery query)
        {
            return RedirectToAction("ShopSearch", "Home", query);
        }

        [HttpGet]
        public async Task<IActionResult> Error()
        {
            if (User.IsInRole("admin"))
            {
                var modelForError = new ModelForError
                {
                    Categories = await Mediator.Send(new GetCategoriesQuery())
                };

                return View("Error", modelForError);
            }
            else if (User.Identity.IsAuthenticated)
            {
                var user = await Mediator.Send(new GetUserQuery { UserName = User.Identity.Name });

                var modelForError = new ModelForError
                {
                    Categories = await Mediator.Send(new GetCategoriesQuery()),
                    ShoppingCart = await Mediator.Send(new GetShoppingCartQuery { Id = user.ShoppingCart.Id })
                };

                return View("Error", modelForError);
            }
            else
            {
                var modelForError = new ModelForError
                {
                    Categories = await Mediator.Send(new GetCategoriesQuery())
                };

                return View("Error", modelForError);
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