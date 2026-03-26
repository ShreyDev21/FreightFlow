using FreightFlow.Shipment.Application.Interfaces;
using FreightFlow.Shipment.Infrastructure.Persistence;
using FreightFlow.Shipment.Infrastructure.Persistence.Repositories;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FreightFlow.Shipment.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // ── Database ──────────────────────────────────────────────
        var connectionString = configuration
            .GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException(
                "Connection string 'DefaultConnection' not found.");

        services.AddDbContext<ShipmentDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<IShipmentRepository, ShipmentRepository>();

        // ── Messaging (MassTransit + RabbitMQ) ───────────────────
        // Why MassTransit? It gives us automatic retries, dead-letter
        // queues, and a clean abstraction over RabbitMQ so we can
        // swap transports later without changing business logic.
        var rabbitMqUri = configuration["RabbitMq:Uri"]
            ?? throw new InvalidOperationException(
                "RabbitMq:Uri configuration not found.");

        services.AddMassTransit(x =>
        {
            // No consumers here — Shipment service only publishes
            // Notification service will have the consumers

            x.UsingRabbitMq((context, cfg) =>
            {
                // Connect to CloudAMQP
                cfg.Host(new Uri(rabbitMqUri));

                // Auto-configure endpoints for any consumers
                cfg.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}