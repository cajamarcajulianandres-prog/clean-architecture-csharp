using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Abstractions;
using Domain.Entities;

namespace Infrastructure.Persistence;

/// <summary>Repositorio en memoria, usado cuando no hay cadena de conexion configurada.</summary>
public sealed class InMemoryOrderRepository : IOrderWriter, IOrderReader
{
    private readonly List<Order> _orders = new();
    private readonly object _gate = new();

    public Task AddAsync(Order order, CancellationToken cancellationToken)
    {
        lock (_gate)
        {
            _orders.Add(order);
        }

        return Task.CompletedTask;
    }

    public Task<IReadOnlyList<Order>> GetLastAsync(int count, CancellationToken cancellationToken)
    {
        lock (_gate)
        {
            IReadOnlyList<Order> result = Enumerable.Reverse(_orders).Take(count).ToList();
            return Task.FromResult(result);
        }
    }
}
