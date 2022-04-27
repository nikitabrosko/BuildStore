using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.UseCases.Category.Commands.CreateCategory;

namespace WebUI.Controllers
{
    public class CategoryController : ApiControllerBase
    {
        public async Task<IActionResult> Index()
        {
            ViewBag.Title = "Home page";

            var category = new CreateCategoryCommand
            {
                Name = "Test",
                Description = "Test",
                Picture = new byte[10]
            };

            await Mediator.Send(category);

            return View();
        }
    }
}
