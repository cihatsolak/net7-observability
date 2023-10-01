using System.ComponentModel.DataAnnotations.Schema;

namespace OrderAPI.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public DateTime CreatedAt { get; set; }
        public int UserId { get; set; }

        public OrderStatus Status { get; set; }
        public List<OrderItem> Items { get; set; }
    }

    public enum OrderStatus : byte
    {
        Fail,
        Success
    }

    public class OrderItem
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }

        [ForeignKey("OrderId")]
        public int OrderId { get; set; }
        public Order Order { get; set; }
    }
}
