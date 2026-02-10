using OrderService.Data;
using OrderService.Models.RequestModels;

namespace OrderService.Repositories;

public class OrderRepository(OrderDbContext context)
{
    private readonly OrderDbContext _context = context;

    public async Task AddAsync(OrderRequest order)
    {
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
    }

    public async Task<OrderRequest?> GetOrderByIdAsync(Guid id)
    {
        return await _context.Orders.FindAsync(id);
    }
}

