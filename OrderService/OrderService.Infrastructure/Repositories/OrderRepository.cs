using Microsoft.EntityFrameworkCore;
using OrderService.Data;
using OrderService.Domain.Entities;

namespace OrderService.Infrastructure.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly OrderDbContext _context;

    public OrderRepository(OrderDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(
        OrderEntity order,
        CancellationToken cancellationToken)
    {
        _context.Orders.Add(order);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<OrderEntity?> GetOrderByIdAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        return await _context.Orders
            .AsNoTracking()
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
    }
}

