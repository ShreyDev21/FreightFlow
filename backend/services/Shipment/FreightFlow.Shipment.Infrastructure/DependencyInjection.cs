using FreightFlow.Shipment.Application.Interfaces;
using FreightFlow.Shipment.Infrastructure.Persistence;
using FreightFlow.Shipment.Infrastructure.Persistence.Repositories;
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
        var connectionString = configuration
            .GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException(
                "Connection string 'DefaultConnection' not found.");

        services.AddDbContext<ShipmentDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<IShipmentRepository, ShipmentRepository>();

        return services;
    }
}