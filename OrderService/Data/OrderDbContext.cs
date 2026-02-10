using Microsoft.EntityFrameworkCore;
using OrderService.Models.Entities;

namespace OrderService.Data;
public class OrderDbContext : DbContext
{
    public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options)
    {
    }

    public DbSet<OrderEntity> Orders => Set<OrderEntity>();
}