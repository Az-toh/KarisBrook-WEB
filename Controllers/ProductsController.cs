using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KarisBrook.Data;
using KarisBrook.Models;

namespace KarisBrook.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult List(string category, string brand, string sortOrder)
        {
            var products = _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .AsQueryable();

            // Фильтрация по категории
            if (!string.IsNullOrEmpty(category))
            {
                products = products.Where(p => p.Category.Slug == category);
            }

            // Фильтрация по бренду
            if (!string.IsNullOrEmpty(brand))
            {
                products = products.Where(p => p.Brand.Name == brand);
            }

            // Сортировка
            products = sortOrder switch
            {
                "price_asc" => products.OrderBy(p => p.Price),
                "price_desc" => products.OrderByDescending(p => p.Price),
                "name" => products.OrderBy(p => p.Name),
                _ => products.OrderByDescending(p => p.Rating)
            };

            var viewModel = new ProductsListViewModel
            {
                Products = products.ToList(),
                Categories = _context.Categories.ToList(),
                Brands = _context.Brands.ToList()
            };

            return View(viewModel);
        }
        public IActionResult Details(int id)
        {
            var product = _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.Reviews) // Когда добавим отзывы
                .FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

    }
}