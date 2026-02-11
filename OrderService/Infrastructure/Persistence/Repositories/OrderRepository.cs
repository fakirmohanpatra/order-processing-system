using Microsoft.EntityFrameworkCore;
using OrderService.Data;
using OrderService.Domain.Entities;
using OrderService.Models.RequestModels;

namespace OrderService.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly OrderDbContext _context;

    public OrderRepository(OrderDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(
        OrderRequest order, 
        CancellationToken cancellationToken)
    {
        var entity = new OrderEntity
        {
            Id = Guid.NewGuid(),
            ProductName = order.ProductName,
            Quantity = order.Quantity,
            Price = order.Price,
            CustomerId = order.CustomerId,
            Status = "Created"
        };

        _context.Orders.Add(entity);
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

