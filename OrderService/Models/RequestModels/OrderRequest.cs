namespace OrderService.Models.RequestModels;  

public record OrderRequest(
    string ProductName,
    int Quantity,
    decimal Price,
    string CustomerId
);