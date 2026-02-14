using Microsoft.Extensions.DependencyInjection;
using OrderService.Application.Commands.CreateOrder;

namespace OrderService.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Application services: handlers will be registered by MediatR

        return services;
    }
}
