using Domain.Common;

namespace Domain.Entities
{
    public class OrderItem : BaseEntity
    {
        public string BookName { get; set; }
        public string Author { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
    }
}
