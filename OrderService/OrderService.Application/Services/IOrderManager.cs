using OrderService.Application.Models.RequestModels;
using OrderService.Application.Models.ResponseModels;

namespace OrderService.Application.Services;

public interface IOrderManager
{
    Task<OrderResponse> AddOrderAsync(OrderRequest request, CancellationToken cancellationToken);
}
