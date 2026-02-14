using MediatR;
using OrderService.Application.Models.ResponseModels;
using OrderService.Domain.Entities;
using OrderService.Domain.Interfaces;

namespace OrderService.Application.Commands.CreateOrder;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, OrderResponse>
{
    private readonly IOrderRepository _repository;

    public CreateOrderCommandHandler(IOrderRepository repository)
    {
        _repository = repository;
    }

    public async Task<OrderResponse> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
    {
        // Create domain entity
        var order = Purchase.Create(
            command.ProductName,
            command.Quantity,
            command.Price,
            command.CustomerId
        );

        // Persist to repository
        await _repository.AddAsync(order, cancellationToken);

        // Return response
        return new OrderResponse
        {
            Id = order.Id,
            Status = order.Status.ToString(),
            CreatedAt = order.CreatedAt
        };
    }
}
