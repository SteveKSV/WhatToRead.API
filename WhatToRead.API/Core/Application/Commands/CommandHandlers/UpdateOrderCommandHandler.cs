using AutoMapper;
using Core.Application.Commands.OrderCommands.UpdateOrder;
using Core.Application.Interfaces;
using Core.Domain.Exceptions;
using MediatR;

namespace Core.Application.Commands.CommandHandlers
{
    public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, int>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public UpdateOrderCommandHandler(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async Task<int> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            var orderEntity = await _orderRepository.GetByIdAsync(request.OrderId);

            if (orderEntity == null)
            {
                // Handle the case where the order is not found
                throw new OrderNotFoundException(request.OrderId);
            }

            // Update the order entity with the new data
            _mapper.Map(request.OrderDto, orderEntity);

            await _orderRepository.UpdateAsync(orderEntity);

            return orderEntity.Id;
        }
    }
}

