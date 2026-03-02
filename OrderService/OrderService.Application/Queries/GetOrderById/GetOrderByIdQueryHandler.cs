using MediatR;
using OrderService.Application.Models.ResponseModels;
using OrderService.Domain.Interfaces;

namespace OrderService.Application.Queries.GetOrderById;

public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, OrderResponse?>
{
    private readonly IOrderRepository _repository;
    public GetOrderByIdQueryHandler(IOrderRepository repository)
    {
        _repository = repository;
    }

    public async Task<OrderResponse?> Handle(
        GetOrderByIdQuery query,
        CancellationToken cancellationToken)
    {
        var order = await _repository.GetOrderByIdAsync(query.purchaseId, cancellationToken);

        if (order == null)
            return null;
        
        return new OrderResponse
        {
            Id = order.Id,
            Status = order.Status.ToString(),
            CreatedAt = DateTime.UtcNow
        };
    }
}