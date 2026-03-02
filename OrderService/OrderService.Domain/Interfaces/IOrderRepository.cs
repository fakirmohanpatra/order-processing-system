using OrderService.Domain.Entities;
using OrderService.Application.Models.ResponseModels;

namespace OrderService.Domain.Interfaces;

public interface IOrderRepository
{
    Task AddAsync(Purchase order, CancellationToken cancellationToken);
    Task<IEnumerable<OrderResponse>> GetAllOrdersAsync(CancellationToken cancellationToken);
    Task<Purchase?> GetOrderByIdAsync(Guid id, CancellationToken cancellationToken);
}
