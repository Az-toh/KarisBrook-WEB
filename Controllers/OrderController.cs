using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KarisBrook.Data;
using KarisBrook.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace KarisBrook.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrderController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: /Order/Checkout
        public async Task<IActionResult> Checkout()
        {
            Console.WriteLine("=== МЕТОД CHECKOUT ВЫЗВАН ===");

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                Console.WriteLine("=== ПОЛЬЗОВАТЕЛЬ НЕ НАЙДЕН ===");
                return Challenge();
            }

            var cartItems = _context.CartItems
                .Include(c => c.Product)
                .Where(c => c.UserId == user.Id)
                .ToList();

            if (!cartItems.Any())
            {
                Console.WriteLine("=== КОРЗИНА ПУСТА ===");
                return RedirectToAction("Index", "Cart");
            }

            Console.WriteLine($"=== В КОРЗИНЕ {cartItems.Count} ТОВАРОВ ===");
            return View(cartItems);
        }

        // POST: /Order/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string shippingAddress, string paymentMethod, string comment)
        {
            Console.WriteLine("=== СОЗДАНИЕ ЗАКАЗА: НАЧАЛО ===");
            Console.WriteLine($"=== АДРЕС: {shippingAddress}, ОПЛАТА: {paymentMethod}, КОММЕНТАРИЙ: {comment} ===");

            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    Console.WriteLine("=== ОШИБКА: ПОЛЬЗОВАТЕЛЬ НЕ НАЙДЕН ===");
                    return Unauthorized();
                }

                var cartItems = _context.CartItems
                    .Include(c => c.Product)
                    .Where(c => c.UserId == user.Id)
                    .ToList();

                if (!cartItems.Any())
                {
                    Console.WriteLine("=== КОРЗИНА ПУСТА ===");
                    return RedirectToAction("Index", "Cart");
                }

                // 1. Создаём заказ
                var order = new Order
                {
                    UserId = user.Id,
                    OrderDate = DateTime.Now,
                    TotalPrice = cartItems.Sum(c => c.Product.Price * c.Quantity),
                    Status = "Новый",
                    ShippingAddress = shippingAddress,
                    PaymentMethod = paymentMethod,
                    Comment = comment
                };

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();
                Console.WriteLine($"=== ЗАКАЗ #{order.Id} СОЗДАН ===");

                // 2. Создаём позиции заказа
                foreach (var item in cartItems)
                {
                    var orderItem = new OrderItem
                    {
                        OrderId = order.Id,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        PriceAtPurchaseTime = item.Product.Price
                    };
                    _context.OrderItems.Add(orderItem);
                }
                await _context.SaveChangesAsync();
                Console.WriteLine($"=== {cartItems.Count} ПОЗИЦИЙ ДОБАВЛЕНО ===");

                // 3. Очищаем корзину
                _context.CartItems.RemoveRange(cartItems);
                await _context.SaveChangesAsync();
                Console.WriteLine("=== КОРЗИНА ОЧИЩЕНА ===");

                Console.WriteLine("=== ЗАКАЗ УСПЕШНО СОЗДАН ===");
                return RedirectToAction("Success", new { id = order.Id });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"=== ОШИБКА ПРИ СОЗДАНИИ ЗАКАЗА: {ex.Message} ===");
                Console.WriteLine($"=== СТЕК: {ex.StackTrace} ===");

                ViewData["ErrorMessage"] = ex.Message;
                return View("Error");
            }
        }

        // GET: /Order/Success/{id}
        public IActionResult Success(int id)
        {
            Console.WriteLine($"=== ПРОСМОТР ЗАКАЗА #{id} ===");

            var order = _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefault(o => o.Id == id);

            if (order == null)
            {
                Console.WriteLine($"=== ЗАКАЗ #{id} НЕ НАЙДЕН ===");
                return NotFound();
            }

            return View(order);
        }

        // GET: /Order/History
        public async Task<IActionResult> History()
        {
            Console.WriteLine("=== ИСТОРИЯ ЗАКАЗОВ ===");

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            var orders = _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Where(o => o.UserId == user.Id)
                .OrderByDescending(o => o.OrderDate)
                .ToList();

            Console.WriteLine($"=== НАЙДЕНО {orders.Count} ЗАКАЗОВ ===");
            return View(orders);
        }
    }
}