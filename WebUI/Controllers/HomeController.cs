using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            if (User.IsInRole("admin"))
            {
                ViewBag.Title = "Home page";

                return View();
            }
            else
            {
                return RedirectToAction("Index", "Category");
            }
        }
    }
}