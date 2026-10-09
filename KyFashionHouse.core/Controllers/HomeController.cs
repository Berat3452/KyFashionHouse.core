using KyFashionHouse.core.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace KyFashionHouse.core.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Tshirtler()
        {
            ViewData["Title"] = "T-Shirtler";
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
