using System.Collections.Generic;

namespace KarisBrook.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal? OldPrice { get; set; } // Может быть null, если нет скидки
        public string ImagePath { get; set; } // Путь к основному изображению
        public double Rating { get; set; } // Рейтинг
        public bool IsNew { get; set; } // Является ли новинкой
        public bool IsSale { get; set; } // Есть ли скидка

        // Внешние ключи
        public int CategoryId { get; set; }
        public int BrandId { get; set; }

        // Навигационные свойства
        public virtual Category Category { get; set; }
        public virtual Brand Brand { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<CartItem> CartItems { get; set; }
    }
}