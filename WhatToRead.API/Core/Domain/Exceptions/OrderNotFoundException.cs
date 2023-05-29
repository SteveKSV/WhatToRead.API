﻿namespace Core.Domain.Exceptions
{
    public class OrderNotFoundException : Exception
    {
        public OrderNotFoundException(int orderId) : base($"Order with id {orderId} was not found.")
        {
        }
    }

}
