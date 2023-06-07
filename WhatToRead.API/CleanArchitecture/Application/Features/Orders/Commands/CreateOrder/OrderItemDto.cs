namespace Application.Features.Orders.Commands.CreateOrder
{
    public class OrderItemDto
    {
        public string BookName { get; set; }
        public string Author { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
