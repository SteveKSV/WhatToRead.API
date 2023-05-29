using Core.Domain.Entities;

namespace Core.Application.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order> GetByIdAsync(int orderId);
        Task<Order> AddAsync(Order order);
        Task UpdateAsync(Order order);
        Task DeleteAsync(int orderId);
    }
}
