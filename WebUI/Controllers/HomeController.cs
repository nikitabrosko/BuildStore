using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.UseCases.Category.Commands.CreateCategory;
using Domain.Entities;

namespace WebUI.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Title = "Home page";

            return View();
        }
    }
}