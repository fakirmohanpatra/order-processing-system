namespace OrderService.Models.ResponseModels;

public record OrderResponse(
    string OrderId,
    string ProductName,
    int Quantity,
    decimal Price,
    string CustomerId,
    string Status
);