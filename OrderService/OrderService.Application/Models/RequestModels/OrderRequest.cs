namespace OrderService.Models.RequestModels;

public class OrderRequest
{
    public string ProductName { get; set; } = default!;
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public string CustomerId { get; set; } = default!;
}
