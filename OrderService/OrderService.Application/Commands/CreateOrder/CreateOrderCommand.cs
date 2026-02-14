using MediatR;
using OrderService.Application.Models.RequestModels;

namespace OrderService.Application.Commands.CreateOrder;

public record CreateOrderCommand(
    string ProductName,
    int Quantity,
    decimal Price,
    string CustomerId
) : IRequest<OrderService.Application.Models.ResponseModels.OrderResponse>
{
    public static CreateOrderCommand FromRequest(OrderRequest request)
    {
        return new CreateOrderCommand(
            request.ProductName,
            request.Quantity,
            request.Price,
            request.CustomerId
        );
    }
}
