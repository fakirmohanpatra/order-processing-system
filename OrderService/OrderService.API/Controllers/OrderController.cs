using Microsoft.AspNetCore.Mvc;
using OrderService.Application.Models.RequestModels;
using OrderService.Application.Models.ResponseModels;
using MediatR;
using OrderService.Application.Commands.CreateOrder;
using OrderService.Application.Queries.GetAllOrders;
using OrderService.Application.Queries.GetOrderById;

namespace OrderService.API.Controllers;

[ApiController]
[Route("api/orders")]
public class OrderController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrderController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<OrderResponse>> CreateOrder(
        [FromBody] OrderRequest orderRequest, 
        CancellationToken cancellationToken)
    {
        var command = CreateOrderCommand.FromRequest(orderRequest);
        var response = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetOrder), new { id = response.Id }, response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<OrderResponse>> GetOrder(
        Guid id,
        CancellationToken cancellationToken)
    {
        var query = new GetOrderByIdQuery(id);
        var response = await _mediator.Send(query, cancellationToken);

        if (response == null)
            return NotFound();
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrderResponse>>> GetAllOrders(
        CancellationToken cancellationToken) 
    {
        var query = new GetAllOrdersQuery();
        var response = await _mediator.Send(query, cancellationToken);

        return Ok(response);
    }
}
