
using KyFashionHouse.Core.Data;
using KyFashionHouse.core.Models;
using Microsoft.AspNetCore.Mvc;

namespace KyFashionHouse.core.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Ürün listeleme sayfası
        public IActionResult Tshirtler()
        {
            ViewData["Title"] = "T-Shirtler";

            List<Product> urunler = _context.Products.ToList();

            return View(urunler);
        }

        // Ürün detay sayfası
        public IActionResult Details(int id)
        {
            var urun = _context.Products
                .FirstOrDefault(p => p.Id == id);

            if (urun == null)
            {
                return NotFound();
            }

            ViewData["Title"] = urun.Name;

            return View(urun);
        }
    }
}
