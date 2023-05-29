using Core.Application.Dtos;

namespace Core.Application.Commands.OrderCommands.CreateOrder
{
    public class CreateOrderCommand
    {
        public string CustomerName { get; set; }
        public List<OrderItemDto> OrderItems { get; set; }
        public object OrderDto { get; internal set; }
        
    }
}
