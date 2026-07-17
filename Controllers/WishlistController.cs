using KarisBrook.Data;
using KarisBrook.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace KarisBrook.Controllers
{
    [Authorize]
    public class WishlistController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public WishlistController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: /Wishlist
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            var wishlistItems = _context.WishlistItems
                .Include(w => w.Product)
                .Where(w => w.UserId == user.Id)
                .OrderByDescending(w => w.AddedDate)
                .ToList();

            return View(wishlistItems);
        }

        // POST: /Wishlist/Add
        [HttpPost]
        public async Task<IActionResult> Add(int productId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var product = _context.Products.Find(productId);
            if (product == null) return NotFound();

            // Проверяем, есть ли товар уже в листе ожидания
            var existingItem = _context.WishlistItems
                .FirstOrDefault(w => w.UserId == user.Id && w.ProductId == productId);

            if (existingItem == null)
            {
                var wishlistItem = new WishlistItem
                {
                    UserId = user.Id,
                    ProductId = productId,
                    AddedDate = DateTime.Now
                };
                _context.WishlistItems.Add(wishlistItem);
                await _context.SaveChangesAsync();

                TempData["Message"] = "Товар добавлен в лист ожидания.";
            }
            else
            {
                TempData["Message"] = "Товар уже в листе ожидания.";
            }

            return RedirectToAction("Index");
        }

        // POST: /Wishlist/Remove
        [HttpPost]
        public async Task<IActionResult> Remove(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var item = _context.WishlistItems
                .FirstOrDefault(w => w.Id == id && w.UserId == user.Id);

            if (item != null)
            {
                _context.WishlistItems.Remove(item);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Товар удалён из листа ожидания.";
            }

            return RedirectToAction("Index");
        }
    }
}