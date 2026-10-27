using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using KyFashionHouse.core.Models;

namespace KyFashionHouse.core.Controllers
{
    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }           
        public IActionResult Sort_Tshirt_Takimlari()
        {
            ViewData["Title"] = "Şort T-Shirt Takımları";
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
