using Core.Domain.Common;
using Core.Domain.Enums;

namespace Core.Domain.Entities
{
    public class Order : BaseEntity
    {
        public DateTime OrderDate { get; set; }
        public OrderStatus Status { get; set; }
        public decimal TotalAmount { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
    }

}
