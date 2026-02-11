using OrderService.Domain.Entities;
using OrderService.Models.RequestModels;

namespace OrderService.Repositories;

public interface IOrderRepository
{
    Task AddAsync(OrderRequest order, CancellationToken cancellationToken);
    Task<OrderEntity?> GetOrderByIdAsync(Guid id, CancellationToken cancellationToken);
}