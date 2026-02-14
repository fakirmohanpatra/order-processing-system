using Microsoft.AspNetCore.Mvc;
using OrderService.Application.Models.RequestModels;
using OrderService.Application.Models.ResponseModels;
using MediatR;
using OrderService.Application.Commands.CreateOrder;

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
    public async Task<ActionResult<OrderResponse>> CreateOrder([FromBody] OrderRequest orderRequest, CancellationToken cancellationToken)
    {
        var command = CreateOrderCommand.FromRequest(orderRequest);
        var response = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(CreateOrder), new { id = response.Id }, response);
    }
}
