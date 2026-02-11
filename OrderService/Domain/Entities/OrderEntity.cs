using OrderService.Domain.Enums;

namespace OrderService.Domain.Entities;

public class OrderEntity
{
    public Guid Id { get; private set; }
    public string ProductName { get; private set; } = default!;
    public int Quantity { get; private set; }
    public decimal Price { get; private set; }
    public string CustomerId { get; private set; } = default!;
    public OrderStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    // Private constructor for EF
    private OrderEntity() {}

    // Factory method
    public static OrderEntity Create(
        string ProductName,
        int Quantity,
        decimal Price,
        string CustomerId)
    {
        if (string.IsNullOrWhiteSpace(ProductName))
            throw new ArgumentException("Product name cannot be empty");

        if (Quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero");
        
        if (Price <= 0)
            throw new ArgumentException("Price must be greater than zero");

        return new OrderEntity
        {
            Id = Guid.NewGuid(),
            ProductName = ProductName,
            Quantity = Quantity,
            Price = Price,
            CustomerId = CustomerId,
            Status = OrderStatus.Created,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void MarkAsPaid()
    {
        if (Status != OrderStatus.Created)
            throw new InvalidOperationException("Order can not be paid in current state");

        Status = OrderStatus.Paid;
    }

    public void Cancel()
    {
        if (Status == OrderStatus.Paid)
            throw new InvalidOperationException("Paid order cannot be cancelled");

        Status = OrderStatus.Cancelled;
    }
}