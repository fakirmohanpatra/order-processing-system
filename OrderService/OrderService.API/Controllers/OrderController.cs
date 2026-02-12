
using Microsoft.AspNetCore.Mvc;
using OrderService.Models.RequestModels;
using OrderService.Models.ResponseModels;
using OrderService.Services;

namespace OrderService.API.Controllers;

[ApiController]
[Route("api/orders")]
public class OrderController : ControllerBase
{
    private readonly IOrderManager _orderManager;

    public OrderController(IOrderManager orderManager)
    {
        _orderManager = orderManager;
    }


    public async Task<ActionResult<OrderResponse>> CreateOrder([FromBody] OrderRequest orderRequest, CancellationToken cancellationToken)
    {
        var response = await _orderManager.AddOrderAsync(orderRequest, cancellationToken);
        return Ok(response);
    }

    
}