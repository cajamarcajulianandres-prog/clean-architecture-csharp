using System;
using Domain.Entities;

namespace Application.UseCases;

public sealed record OrderDto(Guid Id, string CustomerName, string ProductName, int Quantity, decimal UnitPrice, decimal Total)
{
    public static OrderDto From(Order order)
    {
        return new OrderDto(order.Id, order.CustomerName, order.ProductName, order.Quantity, order.UnitPrice, order.Total);
    }
}
