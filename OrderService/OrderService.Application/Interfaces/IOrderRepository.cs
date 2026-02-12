namespace OrderService.OrderService.Application.Interfaces;

public interface IOrderRepository
{
    Task AddAsync(OrderEntity order, CancellationToken cancellationToken);
    Task<OrderEntity?> GetOrderByIdAsync(Guid id, CancellationToken cancellationToken);
}