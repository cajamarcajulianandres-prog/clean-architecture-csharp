using System;
using Domain.Exceptions;

namespace Domain.Entities;

public sealed class Order
{
    private Order(Guid id, string customerName, string productName, int quantity, decimal unitPrice)
    {
        Id = id;
        CustomerName = customerName;
        ProductName = productName;
        Quantity = quantity;
        UnitPrice = unitPrice;
    }

    public Guid Id { get; }
    public string CustomerName { get; }
    public string ProductName { get; }
    public int Quantity { get; }
    public decimal UnitPrice { get; }

    public decimal Total => Quantity * UnitPrice;

    /// Crea un pedido nuevo y valida las reglas de negocio
    public static Order Create(string customerName, string productName, int quantity, decimal unitPrice)
    {
        if (string.IsNullOrWhiteSpace(customerName))
        {
            throw new DomainException("El nombre del cliente es obligatorio.");
        }

        if (string.IsNullOrWhiteSpace(productName))
        {
            throw new DomainException("El nombre del producto es obligatorio.");
        }

        if (quantity <= 0)
        {
            throw new DomainException("La cantidad debe ser mayor que cero.");
        }

        if (unitPrice <= 0)
        {
            throw new DomainException("El precio unitario debe ser mayor que cero.");
        }

        return new Order(Guid.NewGuid(), customerName.Trim(), productName.Trim(), quantity, unitPrice);
    }

    /// <summary>Reconstruye un pedido ya guardado (lo usa la capa de persistencia).</summary>
    public static Order Restore(Guid id, string customerName, string productName, int quantity, decimal unitPrice)
    {
        return new Order(id, customerName, productName, quantity, unitPrice);
    }
}
