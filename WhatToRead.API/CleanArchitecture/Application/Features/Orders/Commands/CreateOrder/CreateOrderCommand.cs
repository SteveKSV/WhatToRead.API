﻿using Application.Interfaces.Repositories;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using MediatR;

namespace Application.Features.Orders.Commands.CreateOrder
{
    public record class CreateOrderCommand : IRequest<Order>
    {
        public DateTime OrderDate { get; set; }
        public OrderStatus Status { get; set; }
        public decimal TotalAmount { get; set; }
        public List<OrderItemDto> OrderItems { get; set; }
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
            var orderItems = request.OrderItems.Select(item => new OrderItem
            {
                BookName = item.BookName,
                Author = item.Author,
                Price = item.Price,
                Quantity = item.Quantity
            }).ToList();

            var order = new Order()
            {
                OrderDate = request.OrderDate,
                Status = request.Status,
                TotalAmount = request.TotalAmount,
                OrderItems = orderItems
            };

            await _unitOfWork.Repository<Order>().AddAsync(order);
            await _unitOfWork.Save(cancellationToken);

            return order;
        }
    }
}
