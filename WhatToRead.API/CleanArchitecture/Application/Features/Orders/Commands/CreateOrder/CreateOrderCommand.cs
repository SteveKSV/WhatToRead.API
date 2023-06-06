using Application.Interfaces.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.Orders.Commands.CreateOrder
{
    public record class CreateOrderCommand : IRequest<Order>
    {
        public Order Order { get; set; }
    }

    internal class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Order>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateOrderCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Order> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var order = new Order()
            {
                OrderDate = request.Order.OrderDate,
                Status = request.Order.Status,
                TotalAmount = request.Order.TotalAmount,
                OrderItems = request.Order.OrderItems
            };

            await _unitOfWork.Repository<Order>().AddAsync(order);
            await _unitOfWork.Save(cancellationToken);
            return order;
        }
    }
}
