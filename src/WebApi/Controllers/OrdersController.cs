using Application.UseCases;
using Application.UseCases.CreateOrder;
using Application.UseCases.GetLastOrders;
using Microsoft.AspNetCore.Mvc;
using WebApi.Contracts;

namespace WebApi.Controllers;

[ApiController]
[Route("orders")]
public sealed class OrdersController : ControllerBase
{
    private readonly CreateOrderUseCase _createOrder;
    private readonly GetLastOrdersUseCase _getLastOrders;

    public OrdersController(CreateOrderUseCase createOrder, GetLastOrdersUseCase getLastOrders)
    {
        _createOrder = createOrder;
        _getLastOrders = getLastOrders;
    }

    [HttpPost]
    public async Task<ActionResult<OrderDto>> Create([FromBody] CreateOrderRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateOrderCommand(request.CustomerName, request.ProductName, request.Quantity, request.UnitPrice);
        var order = await _createOrder.ExecuteAsync(command, cancellationToken);
        return StatusCode(StatusCodes.Status201Created, order);
    }

    [HttpGet("last")]
    public async Task<ActionResult<IReadOnlyList<OrderDto>>> GetLast([FromQuery] int count = 10, CancellationToken cancellationToken = default)
    {
        var orders = await _getLastOrders.ExecuteAsync(count, cancellationToken);
        return Ok(orders);
    }
}
