using Domain.Entities;
using Domain.Exceptions;
using Xunit;

namespace Application.Tests;

public class OrderTests
{
    [Fact]
    public void Create_WithValidData_CalculatesTotal()
    {
        var order = Order.Create("Ana", "Teclado", 3, 10.5m);

        Assert.Equal(31.5m, order.Total);
        Assert.NotEqual(System.Guid.Empty, order.Id);
    }

    [Theory]
    [InlineData("", "Teclado", 1, 5)]
    [InlineData("Ana", " ", 1, 5)]
    [InlineData("Ana", "Teclado", 0, 5)]
    [InlineData("Ana", "Teclado", 1, 0)]
    public void Create_WithInvalidData_Throws(string customer, string product, int qty, decimal price)
    {
        Assert.Throws<DomainException>(() => Order.Create(customer, product, qty, price));
    }
}
