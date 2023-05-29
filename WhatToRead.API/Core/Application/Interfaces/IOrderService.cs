using Core.Application.Dtos;

namespace Core.Application.Interfaces
{
    public interface IOrderService
    {
        Task<OrderDto> GetOrderByIdAsync(int orderId);
        Task<int> CreateOrderAsync(OrderDto orderDto);
        Task UpdateOrderAsync(OrderDto orderDto);
        Task DeleteOrderAsync(int orderId);
    }
}
