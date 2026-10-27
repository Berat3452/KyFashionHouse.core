
using KyFashionHouse.Core.Data;
using KyFashionHouse.core.Models;
using KyFashionHouse.core.Models.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KyFashionHouse.core.Controllers
{
    [Authorize]
    public class CheckoutController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public CheckoutController(
            ApplicationDbContext context,
            UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var sepet = HttpContext.Session.GetObject<List<CartItem>>("Sepet")
                        ?? new List<CartItem>();

            if (sepet.Count == 0)
            {
                return RedirectToAction("Sepet", "Cart");
            }

            return View(new CheckoutViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(CheckoutViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var sepet = HttpContext.Session.GetObject<List<CartItem>>("Sepet")
                        ?? new List<CartItem>();

            if (sepet.Count == 0)
            {
                return RedirectToAction("Sepet", "Cart");
            }

            var kullaniciId = _userManager.GetUserId(User);

            if (kullaniciId == null)
            {
                return Challenge();
            }

            var siparis = new Order
            {
                UserId = kullaniciId,
                FullName = model.FullName,
                Phone = model.Phone,
                ShippingAddress = model.ShippingAddress,
                OrderDate = DateTime.Now,
                Status = "Beklemede",
                TotalAmount = 0
            };

            foreach (var sepetUrunu in sepet)
            {
                var urun = _context.Products
                    .FirstOrDefault(p => p.Id == sepetUrunu.ProductId);

                if (urun == null || urun.Stock < sepetUrunu.Quantity)
                {
                    TempData["Mesaj"] =
                        "Bir ürünün stoğu yetersiz veya ürün artık mevcut değil.";

                    return RedirectToAction("Sepet", "Cart");
                }

                var siparisKalemi = new OrderItem
                {
                    ProductId = urun.Id,
                    Quantity = sepetUrunu.Quantity,
                    UnitPrice = urun.Price
                };

                siparis.OrderItems.Add(siparisKalemi);
                siparis.TotalAmount += urun.Price * sepetUrunu.Quantity;
            }

            using var transaction = _context.Database.BeginTransaction();

            try
            {
                foreach (var siparisKalemi in siparis.OrderItems)
                {
                    var urun = _context.Products
                        .First(p => p.Id == siparisKalemi.ProductId);

                    urun.Stock -= siparisKalemi.Quantity;
                }

                _context.Orders.Add(siparis);
                _context.SaveChanges();

                transaction.Commit();

                HttpContext.Session.Remove("Sepet");

                TempData["Mesaj"] = "Siparişiniz başarıyla oluşturuldu.";

                return RedirectToAction("SiparisBasarili");
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        [HttpGet]
        public IActionResult SiparisBasarili()
        {
            return View();
        }
    }
}
