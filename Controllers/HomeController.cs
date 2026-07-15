using KarisBrook.Data;
using KarisBrook.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace KarisBrook.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var productCount = _context.Products.Count();
            Console.WriteLine($"=== Найдено товаров в БД: {productCount} ===");

            var viewModel = new HomeViewModel
            {
                BestSellers = _context.Products
                    .OrderByDescending(p => p.Rating)
                    .Take(6)
                    .Include(p => p.Brand)
                    .ToList()
            };

            Console.WriteLine($"=== Передано товаров в представление: {viewModel.BestSellers.Count()} ===");
            return View(viewModel);
        }
    }
}