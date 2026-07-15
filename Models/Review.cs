using System;

namespace KarisBrook.Models
{
    public class Review
    {
        public int Id { get; set; }
        public string UserName { get; set; } // Имя автора отзыва
        public int Rating { get; set; } // Оценка от 1 до 5
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Внешние ключи
        public int ProductId { get; set; }
        public string UserId { get; set; } // Если вы используете Identity

        // Навигационные свойства
        public virtual Product Product { get; set; }
        public virtual ApplicationUser User { get; set; } // Позже заменим на ApplicationUser
    }
}
namespace Karisbrook.Models
{
    public class Review
    {
    }
}
