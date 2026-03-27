using FreightFlow.Tracking.Application.Interfaces;
using FreightFlow.Tracking.Domain.Entities;
using StackExchange.Redis;
using System.Text.Json;

namespace FreightFlow.Tracking.Infrastructure.Repositories;

public sealed class LocationRepository : ILocationRepository
{
    private readonly IConnectionMultiplexer _redis;

    // Key pattern: location:{shipmentId}
    // Why a pattern? Consistent keys prevent typos and make
    // bulk operations easy (e.g. scan all location:* keys)
    private static string LocationKey(Guid shipmentId) =>
        $"location:{shipmentId}";

    public LocationRepository(IConnectionMultiplexer redis)
    {
        _redis = redis;
    }

    public async Task SaveAsync(
        LocationUpdate location,
        CancellationToken cancellationToken = default)
    {
        var db = _redis.GetDatabase();

        // Serialize the location to JSON for storage
        var json = JsonSerializer.Serialize(new
        {
            location.ShipmentId,
            location.Latitude,
            location.Longitude,
            location.RecordedAt
        });

        // Store with 1 hour TTL — if driver stops sending,
        // the stale location automatically disappears
        await db.StringSetAsync(
            LocationKey(location.ShipmentId),
            json,
            TimeSpan.FromHours(1));
    }

    public async Task<LocationUpdate?> GetLatestAsync(
        Guid shipmentId,
        CancellationToken cancellationToken = default)
    {
        var db = _redis.GetDatabase();
        var json = await db.StringGetAsync(LocationKey(shipmentId));

        if (json.IsNullOrEmpty)
            return null;

        // Deserialize back to a LocationUpdate
        var data = JsonSerializer.Deserialize<LocationData>(json!);
        if (data is null) return null;

        return LocationUpdate.Create(
            data.ShipmentId,
            data.Latitude,
            data.Longitude);
    }

    // Private record for deserialization
    private sealed record LocationData(
        Guid ShipmentId,
        double Latitude,
        double Longitude,
        DateTime RecordedAt);
}