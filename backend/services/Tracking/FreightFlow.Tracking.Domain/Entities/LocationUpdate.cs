namespace FreightFlow.Tracking.Domain.Entities;

// Represents a single GPS ping from a driver
// Immutable — a location at a point in time never changes
public sealed class LocationUpdate
{
    public Guid Id { get; private set; }
    public Guid ShipmentId { get; private set; }
    public double Latitude { get; private set; }
    public double Longitude { get; private set; }
    public DateTime RecordedAt { get; private set; }

    private LocationUpdate() { }

    public static LocationUpdate Create(
        Guid shipmentId,
        double latitude,
        double longitude)
    {
        // Basic coordinate validation
        if (latitude < -90 || latitude > 90)
            throw new ArgumentException("Latitude must be between -90 and 90.", nameof(latitude));

        if (longitude < -180 || longitude > 180)
            throw new ArgumentException("Longitude must be between -180 and 180.", nameof(longitude));

        return new LocationUpdate
        {
            Id = Guid.NewGuid(),
            ShipmentId = shipmentId,
            Latitude = latitude,
            Longitude = longitude,
            RecordedAt = DateTime.UtcNow
        };
    }
}