namespace FreightFlow.Shipment.Application.DTOs;

public sealed class ShipmentDto
{
    public Guid Id { get; init; }
    public string TrackingCode { get; init; } = string.Empty;
    public string SenderName { get; init; } = string.Empty;
    public string ReceiverName { get; init; } = string.Empty;
    public string OriginAddress { get; init; } = string.Empty;
    public string DestinationAddress { get; init; } = string.Empty;
    public decimal WeightKg { get; init; }
    public string Status { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }
}