using KarisBrook.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KarisBrook.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            // Ensure the database is created
            context.Database.EnsureCreated();

            // Look for any categories.
            if (context.Products.Any())
            {
                return;   // DB has been seeded
            }

            // 1. Create Brands
            var brands = new Brand[]
            {
                new Brand { Name = "Arthur Ashe", LogoPath = "/brand logo/1.png" },
                new Brand { Name = "Rowing Blazers", LogoPath = "/brand logo/6.png" },
                new Brand { Name = "Strellson", LogoPath = "/brand logo/10.png" },
                new Brand { Name = "Gant", LogoPath = "/brand logo/11.png" },
                new Brand { Name = "D.Molina", LogoPath = "/brand logo/7.png" },
            };
            context.Brands.AddRange(brands);
            context.SaveChanges();

            // 2. Create Categories
            var categories = new Category[]
            {
                new Category { Name = "Поло", Slug = "polo" },
                new Category { Name = "Свитера", Slug = "sweater" },
                new Category { Name = "Рубашки", Slug = "shirts" },
                new Category { Name = "Куртки", Slug = "jackets" },
                new Category { Name = "Шорты", Slug = "shorts" },
                new Category { Name = "Футболки", Slug = "tee" },
            };
            context.Categories.AddRange(categories);
            context.SaveChanges();

            // 3. Create Products
            var products = new Product[]
            {
                new Product {
                    Name = "Поло Ashe Racquets",
                    Description = "Топ-поло для тенниса с жаккардовым рисунком из смеси хлопка и вискозы 'Птичий глаз'.",
                    Price = 5200m,
                    ImagePath = "/product/1,1.jpg",
                    Rating = 4.5,
                    IsNew = true,
                    IsSale = false,
                    CategoryId = categories.First(c => c.Slug == "polo").Id,
                    BrandId = brands.First(b => b.Name == "Arthur Ashe").Id
                },
                new Product {
                    Name = "Свитер Arthur Ashe",
                    Description = "Хлопковый свитер с высоким воротом и вышитым значком Артура Эша.",
                    Price = 5400m,
                    ImagePath = "/product/2,1.jpg",
                    Rating = 5.0,
                    IsNew = true,
                    IsSale = false,
                    CategoryId = categories.First(c => c.Slug == "sweater").Id,
                    BrandId = brands.First(b => b.Name == "Arthur Ashe").Id
                },
                new Product {
                    Name = "Шорты Ashe Paisley",
                    Description = "Шорты из вискозы с принтом, напоминающим шелк, с оригинальным узором пейсли.",
                    Price = 900m,
                    OldPrice = 1000m,
                    ImagePath = "/product/8,1.jpg",
                    Rating = 4.5,
                    IsNew = true,
                    IsSale = true,
                    CategoryId = categories.First(c => c.Slug == "shorts").Id,
                    BrandId = brands.First(b => b.Name == "Arthur Ashe").Id
                }
            };
            context.Products.AddRange(products);
            context.SaveChanges();
        }
    }
}