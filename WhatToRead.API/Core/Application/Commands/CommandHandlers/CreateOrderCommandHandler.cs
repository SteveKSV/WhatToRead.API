using AutoMapper;
using Core.Application.Commands.OrderCommands.CreateOrder;
using Core.Application.Interfaces;
using Core.Domain.Entities;
using MediatR;

namespace Core.Application.Commands.CommandHandlers
{
    public class CreateOrderCommandHandler : IRequest<int>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public CreateOrderCommandHandler(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var orderEntity = _mapper.Map<Order>(request.OrderDto);
            var createdOrder = await _orderRepository.AddAsync(orderEntity);

            return createdOrder.Id;
        }
    }
}
