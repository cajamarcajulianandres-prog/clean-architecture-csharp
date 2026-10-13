namespace Application.UseCases.CreateOrder;

public sealed record CreateOrderCommand(string CustomerName, string ProductName, int Quantity, decimal UnitPrice);
