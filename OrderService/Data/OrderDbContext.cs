using Microsoft.EntityFrameworkCore;
using OrderService.Models.RequestModels;

namespace OrderService.Data;
public class OrderDbContext : DbContext
{
    public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options)
    {
    }

    public DbSet<OrderRequest> Orders { get; set; }
}