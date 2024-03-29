﻿using Application.Features.Orders.Commands.CreateOrder;
using Domain.Enums;

namespace Application.Features.Orders.Queries.GetAllOrders
{
    public class GetAllOrdersDto
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStatus Status { get; set; }
        public string StatusString { get; set; }
        public decimal TotalAmount { get; set; }
        public ICollection<OrderItemDto> OrderItems { get; set; }
    }
}
