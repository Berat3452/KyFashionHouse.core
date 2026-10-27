
using KyFashionHouse.Core.Data;
using KyFashionHouse.core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KyFashionHouse.core.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public OrdersController(
            ApplicationDbContext context,
            UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Authorize]
        public IActionResult Siparislerim()
        {
            var kullaniciId = _userManager.GetUserId(User);

            var siparisler = _context.Orders
                .Where(s => s.UserId == kullaniciId)
                .OrderByDescending(s => s.OrderDate)
                .ToList();

            return View(siparisler);
        }
    }
}
