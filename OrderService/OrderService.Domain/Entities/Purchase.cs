using OrderService.Domain.Enums;

namespace OrderService.Domain.Entities;

public class Purchase
{
    public Guid Id { get; private set; }
    public string ProductName { get; private set; } = default!;
    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }
    public string CustomerId { get; private set; } = default!;
    public PurchaseStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    // Private constructor for EF
    private Purchase() { }

    private Purchase(
        string productName,
        int quantity,
        decimal unitPrice,
        string customerId)
    {
        Id = Guid.NewGuid();
        ProductName = productName;
        Quantity = quantity;
        UnitPrice = unitPrice;
        CustomerId = customerId;
        Status = PurchaseStatus.Created;
        CreatedAt = DateTime.UtcNow;
    }

    // Factory method
    public static Purchase Create(
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

        return new Purchase
        {
            Id = Guid.NewGuid(),
            ProductName = ProductName,
            Quantity = Quantity,
            UnitPrice = Price,
            CustomerId = CustomerId,
            Status = PurchaseStatus.Created,
            CreatedAt = DateTime.UtcNow
        };
    }

    public decimal GetTotalAmount()
    {
        return Quantity * UnitPrice;
    }
    public void MarkAsPaid()
    {
        if (Status != PurchaseStatus.Created)
            throw new InvalidOperationException("Order can not be paid in current state");

        Status = PurchaseStatus.Paid;
    }

    public void Cancel()
    {
        if (Status == PurchaseStatus.Paid)
            throw new InvalidOperationException("Paid order cannot be cancelled");

        Status = PurchaseStatus.Cancelled;
    }
}