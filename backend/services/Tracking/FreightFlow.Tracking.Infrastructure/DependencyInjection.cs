using FreightFlow.Tracking.Application.Interfaces;
using FreightFlow.Tracking.Infrastructure.Repositories;
using FreightFlow.Tracking.Infrastructure.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace FreightFlow.Tracking.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddTrackingInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // ── Redis (Upstash) ───────────────────────────────────────
        // Upstash requires SSL — the configurationOptions handles that
        var redisUrl = configuration["Redis:Url"]
            ?? throw new InvalidOperationException("Redis:Url not found.");

        var redisToken = configuration["Redis:Token"]
            ?? throw new InvalidOperationException("Redis:Token not found.");

        // Upstash uses token-based auth via the password field
        var redisConfig = new ConfigurationOptions
        {
            EndPoints = { redisUrl.Replace("https://", "") },
            Password = redisToken,
            Ssl = true,
            AbortOnConnectFail = false
        };

        services.AddSingleton<IConnectionMultiplexer>(
            ConnectionMultiplexer.Connect(redisConfig));

        services.AddScoped<ILocationRepository, LocationRepository>();

        // ── SignalR ───────────────────────────────────────────────
        // ITrackingHubService bridges Application → SignalR hub
        services.AddScoped<ITrackingHubService, TrackingHubService>();

        return services;
    }
}