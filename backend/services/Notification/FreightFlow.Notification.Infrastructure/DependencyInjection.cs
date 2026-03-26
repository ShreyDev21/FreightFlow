using FreightFlow.Notification.Application.Handlers;
using FreightFlow.Notification.Infrastructure.Consumers;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FreightFlow.Notification.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddNotificationInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Register the chain builder and its handlers
        // Transient because each message gets a fresh chain
        services.AddTransient<NotificationChainBuilder>();

        var rabbitMqUri = configuration["RabbitMq:Uri"]
            ?? throw new InvalidOperationException(
                "RabbitMq:Uri configuration not found.");

        services.AddMassTransit(x =>
        {
            // Register our consumer
            x.AddConsumer<ShipmentCreatedConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(new Uri(rabbitMqUri));

                // Auto-configure queue for ShipmentCreatedConsumer
                // MassTransit will create a queue named:
                // "freightflow.notification.infrastructure.consumers:shipment-created"
                cfg.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}