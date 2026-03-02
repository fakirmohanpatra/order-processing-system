using MediatR;
using OrderService.Application.Models.ResponseModels;
using OrderService.Domain.Interfaces;

namespace OrderService.Application.Queries.GetAllOrders;

public class GetAllOrdersQueryHandler : IRequestHandler<GetAllOrdersQuery, IEnumerable<OrderResponse>>
{
    private readonly IOrderRepository _repository;

    public GetAllOrdersQueryHandler(IOrderRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<OrderResponse>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
    {
        var orders = await _repository.GetAllOrdersAsync(cancellationToken);

        return [.. orders.Select(o => new OrderResponse
        {
            Id = o.Id,
            Status = o.Status.ToString(),
            CreatedAt = o.CreatedAt
        })];
    }
}