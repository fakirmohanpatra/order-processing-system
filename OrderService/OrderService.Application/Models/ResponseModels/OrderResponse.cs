namespace OrderService.Models.ResponseModels;

public class OrderResponse
{
    public Guid Id { get; set; }
    public string Status { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
}
