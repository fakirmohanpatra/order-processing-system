using OrderService.Models.RequestModels;
using OrderService.Models.ResponseModels;

namespace OrderService.Services;

public interface IOrderManager
{
    Task<OrderResponse> AddOrderAsync(OrderRequest request, CancellationToken cancellationToken);
}
