namespace FreightFlow.Tracking.Application.DTOs;

public sealed class LocationUpdateDto
{
    public Guid ShipmentId { get; init; }
    public double Latitude { get; init; }
    public double Longitude { get; init; }
    public DateTime RecordedAt { get; init; }
}