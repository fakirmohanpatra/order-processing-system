using Microsoft.EntityFrameworkCore;
using OrderService.Domain.Interfaces;
using OrderService.Domain.Entities;
using OrderService.Infrastructure.Persistence;

namespace OrderService.Infrastructure.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _context;

    public OrderRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(
        Purchase order,
        CancellationToken cancellationToken)
    {
        _context.Purchases.Add(order);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Purchase?> GetOrderByIdAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        return await _context.Purchases
            .AsNoTracking()
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
    }
}

