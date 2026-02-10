using OrderService.Models.RequestModels;
using OrderService.Models.ResponseModels;
using OrderService.Repositories;

namespace OrderService.Services;

public class OrderManager
{
    private readonly OrderRepository _orderRepository;

    public OrderManager(OrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<OrderResponse> AddOrderAsync(OrderRequest orderRequest)
    {
        // Persist the request through repository (DB or in-memory)
        await _orderRepository.AddAsync(orderRequest);

        // response
        var response = new OrderResponse(
            Guid.NewGuid().ToString(),
            orderRequest.ProductName,
            orderRequest.Quantity,
            orderRequest.Price,
            orderRequest.CustomerId,
            "Created"
        );

        return response;
    }
}