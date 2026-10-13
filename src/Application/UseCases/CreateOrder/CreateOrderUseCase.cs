using System.Threading;
using System.Threading.Tasks;
using Application.Abstractions;
using Domain.Entities;

namespace Application.UseCases.CreateOrder;

public sealed class CreateOrderUseCase
{
    private readonly IOrderWriter _orderWriter;
    private readonly IAppLogger _logger;

    public CreateOrderUseCase(IOrderWriter orderWriter, IAppLogger logger)
    {
        _orderWriter = orderWriter;
        _logger = logger;
    }

    public async Task<OrderDto> ExecuteAsync(CreateOrderCommand command, CancellationToken cancellationToken)
    {
        var order = Order.Create(command.CustomerName, command.ProductName, command.Quantity, command.UnitPrice);

        await _orderWriter.AddAsync(order, cancellationToken);

        _logger.Info($"Order {order.Id} created");
        return OrderDto.From(order);
    }
}
