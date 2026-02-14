using OrderService.Domain.Entities;

namespace OrderService.Domain.Interfaces;

public interface IOrderRepository
{
    Task AddAsync(Purchase order, CancellationToken cancellationToken);
    Task<Purchase?> GetOrderByIdAsync(Guid id, CancellationToken cancellationToken);
}
