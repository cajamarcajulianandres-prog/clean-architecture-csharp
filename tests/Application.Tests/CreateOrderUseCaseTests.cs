using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Abstractions;
using Application.UseCases.CreateOrder;
using Domain.Entities;
using Domain.Exceptions;
using Xunit;

namespace Application.Tests;

public class CreateOrderUseCaseTests
{
    private sealed class FakeWriter : IOrderWriter
    {
        public List<Order> Saved { get; } = new();

        public Task AddAsync(Order order, CancellationToken cancellationToken)
        {
            Saved.Add(order);
            return Task.CompletedTask;
        }
    }

    private sealed class FakeLogger : IAppLogger
    {
        public void Info(string message)
        {
        }

        public void Error(string message, Exception exception)
        {
        }
    }

    [Fact]
    public async Task Execute_SavesOrderAndReturnsDto()
    {
        var writer = new FakeWriter();
        var useCase = new CreateOrderUseCase(writer, new FakeLogger());

        var dto = await useCase.ExecuteAsync(new CreateOrderCommand("Ana", "Teclado", 2, 10m), CancellationToken.None);

        Assert.Single(writer.Saved);
        Assert.Equal(20m, dto.Total);
    }

    [Fact]
    public async Task Execute_WithInvalidQuantity_DoesNotSave()
    {
        var writer = new FakeWriter();
        var useCase = new CreateOrderUseCase(writer, new FakeLogger());

        await Assert.ThrowsAsync<DomainException>(() =>
            useCase.ExecuteAsync(new CreateOrderCommand("Ana", "Teclado", 0, 10m), CancellationToken.None));

        Assert.Empty(writer.Saved);
    }
}
