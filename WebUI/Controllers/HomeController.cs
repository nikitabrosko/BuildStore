using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.Controllers
{
    public class HomeController : ApiControllerBase
    {
        public IActionResult Index()
        {
            ViewBag.Title = "Home page";

            return View();
        }
    }
}
