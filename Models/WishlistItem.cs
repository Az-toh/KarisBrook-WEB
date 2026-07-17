using System;

namespace KarisBrook.Models
{
    public class WishlistItem
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int ProductId { get; set; }
        public DateTime AddedDate { get; set; } = DateTime.Now;

        // Навигационные свойства
        public virtual ApplicationUser User { get; set; }
        public virtual Product Product { get; set; }
    }
}

