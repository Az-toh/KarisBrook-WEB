using System.Collections.Generic;

namespace KarisBrook.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; } // Для ЧПУ-адресов, например, "clothing"
        public int? ParentCategoryId { get; set; } // Для иерархии (например, Одежда -> Поло)

        // Навигационные свойства
        public virtual Category ParentCategory { get; set; }
        public virtual ICollection<Category> ChildCategories { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
namespace Karisbrook.Models
{
    public class Category
    {
    }
}
