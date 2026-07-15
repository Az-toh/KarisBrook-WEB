using KarisBrook;

namespace KarisBrook.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public string SelectedColor { get; set; }
        public string SelectedSize { get; set; }

        // Внешние ключи
        public int ProductId { get; set; }
        public string UserId { get; set; } // Привязка к пользователю

        // Навигационные свойства
        public virtual Product Product { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
namespace Karisbrook.Models
{
    public class CartItem
    {
    }
}
