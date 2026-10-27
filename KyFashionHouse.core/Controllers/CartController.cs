
using KyFashionHouse.Core.Data;
using KyFashionHouse.core.Models;
using KyFashionHouse.core.Models.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace KyFashionHouse.core.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Sepeti görüntüle
        public IActionResult Sepet()
        {
            var sepet = HttpContext.Session
                .GetObject<List<CartItem>>("Sepet")
                ?? new List<CartItem>();

            return View(sepet);
        }

        // Sepete ürün ekle
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SepeteEkle(int id)
        {
            var urun = _context.Products.FirstOrDefault(p => p.Id == id);

            if (urun == null)
            {
                return NotFound();
            }

            if (urun.Stock <= 0)
            {
                TempData["Mesaj"] = "Bu ürün stokta bulunmuyor.";
                return RedirectToAction("Details", "Products",
                    new { id = id });
            }

            var sepet = HttpContext.Session
                .GetObject<List<CartItem>>("Sepet")
                ?? new List<CartItem>();

            var sepettekiUrun =
                sepet.FirstOrDefault(p => p.ProductId == urun.Id);

            if (sepettekiUrun != null)
            {
                if (sepettekiUrun.Quantity >= urun.Stock)
                {
                    TempData["Mesaj"] =
                        "Stoktaki ürün adedine ulaştınız.";

                    return RedirectToAction("Details", "Products",
                        new { id = id });
                }

                sepettekiUrun.Quantity++;
            }
            else
            {
                sepet.Add(new CartItem
                {
                    ProductId = urun.Id,
                    Name = urun.Name,
                    Price = urun.Price,
                    ImageUrl = urun.ImageUrl,
                    Quantity = 1
                });
            }

            HttpContext.Session.SetObject("Sepet", sepet);

            TempData["Mesaj"] = "Ürün sepete eklendi.";

            return RedirectToAction("Sepet");
        }

        // Ürün adedini artır
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AdetArtir(int id)
        {
            var sepet = HttpContext.Session
                .GetObject<List<CartItem>>("Sepet")
                ?? new List<CartItem>();

            var sepetUrunu =
                sepet.FirstOrDefault(u => u.ProductId == id);

            if (sepetUrunu != null)
            {
                var urun = _context.Products
                    .FirstOrDefault(u => u.Id == id);

                if (urun != null && sepetUrunu.Quantity < urun.Stock)
                {
                    sepetUrunu.Quantity++;
                }
                else
                {
                    TempData["Mesaj"] =
                        "Stokta daha fazla ürün bulunmuyor.";
                }
            }

            HttpContext.Session.SetObject("Sepet", sepet);

            return RedirectToAction("Sepet");
        }

        // Ürün adedini azalt
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AdetAzalt(int id)
        {
            var sepet = HttpContext.Session
                .GetObject<List<CartItem>>("Sepet")
                ?? new List<CartItem>();

            var sepetUrunu =
                sepet.FirstOrDefault(u => u.ProductId == id);

            if (sepetUrunu != null)
            {
                sepetUrunu.Quantity--;

                if (sepetUrunu.Quantity <= 0)
                {
                    sepet.Remove(sepetUrunu);
                }
            }

            HttpContext.Session.SetObject("Sepet", sepet);

            return RedirectToAction("Sepet");
        }

        // Ürünü sepetten sil
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SepettenSil(int id)
        {
            var sepet = HttpContext.Session
                .GetObject<List<CartItem>>("Sepet")
                ?? new List<CartItem>();

            var sepetUrunu =
                sepet.FirstOrDefault(u => u.ProductId == id);

            if (sepetUrunu != null)
            {
                sepet.Remove(sepetUrunu);
            }

            HttpContext.Session.SetObject("Sepet", sepet);

            return RedirectToAction("Sepet");
        }
    }
}
