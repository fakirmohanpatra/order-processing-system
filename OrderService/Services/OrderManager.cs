using OrderService.Models.RequestModels;
using OrderService.Models.ResponseModels;
using OrderService.Repositories;

namespace OrderService.Services;

public class OrderManager : IOrderManager
{
    private readonly IOrderRepository _orderRepository;

    public OrderManager(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<OrderResponse> AddOrderAsync(OrderRequest orderRequest, CancellationToken cancellationToken)
    {
        // Respect cancellation early
        cancellationToken.ThrowIfCancellationRequested();

        // Persist the request through repository (DB or in-memory)
        await _orderRepository.AddAsync(orderRequest, cancellationToken);

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