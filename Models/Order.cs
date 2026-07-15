using System;
using System.Collections.Generic;

namespace KarisBrook.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string UserId { get; set; }               // Кто заказал
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public decimal TotalPrice { get; set; }          // Общая сумма заказа
        public string Status { get; set; } = "Новый";    // Статус заказа
        public string ShippingAddress { get; set; }      // Адрес доставки
        public string PaymentMethod { get; set; }        // Способ оплаты
        public string Comment { get; set; }              // Комментарий к заказу

        // Навигационные свойства
        public virtual ApplicationUser User { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}