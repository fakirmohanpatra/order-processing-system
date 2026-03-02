using MediatR;
using OrderService.Application.Models.ResponseModels;

namespace OrderService.Application.Queries.GetAllOrders;

public record GetAllOrdersQuery : IRequest<IEnumerable<OrderResponse>>;