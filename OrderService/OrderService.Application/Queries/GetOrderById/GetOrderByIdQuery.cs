using MediatR;
using OrderService.Application.Models.ResponseModels;

namespace OrderService.Application.Queries.GetOrderById;

public record GetOrderByIdQuery(Guid purchaseId) : IRequest<OrderResponse?>;