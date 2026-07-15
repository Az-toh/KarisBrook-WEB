namespace KarisBrook.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }                 // ID заказа
        public int ProductId { get; set; }               // ID товара
        public int Quantity { get; set; }                // Количество
        public decimal PriceAtPurchaseTime { get; set; } // Цена на момент покупки

        // Навигационные свойства
        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
    }
}