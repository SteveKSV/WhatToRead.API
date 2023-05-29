using Core.Application.Dtos;
using MediatR;

namespace Core.Application.Commands.OrderCommands.UpdateOrder
{
    public class UpdateOrderCommand : IRequest<int>
    {
        public int OrderId { get; set; }
        public List<OrderItemDto> UpdatedOrderItems { get; set; }
        public object OrderDto { get; internal set; }
    }
}
