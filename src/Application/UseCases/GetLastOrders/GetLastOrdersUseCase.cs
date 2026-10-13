using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Abstractions;

namespace Application.UseCases.GetLastOrders;

public sealed class GetLastOrdersUseCase
{
    private const int MaxCount = 100;

    private readonly IOrderReader _orderReader;

    public GetLastOrdersUseCase(IOrderReader orderReader)
    {
        _orderReader = orderReader;
    }

    public async Task<IReadOnlyList<OrderDto>> ExecuteAsync(int count, CancellationToken cancellationToken)
    {
        var safeCount = Math.Clamp(count, 1, MaxCount);
        var orders = await _orderReader.GetLastAsync(safeCount, cancellationToken);
        return orders.Select(OrderDto.From).ToList();
    }
}
