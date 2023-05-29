using Core.Domain.Enums;

namespace Core.Application.Dtos
{
    public class OrderDto
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStatus Status { get; set; }
        public decimal TotalAmount { get; set; }
        public List<OrderItemDto> OrderItems { get; set; }
    }
}
