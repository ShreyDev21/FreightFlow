namespace FreightFlow.Shared.Contracts.Events;

// This is the message contract — both publisher and consumer
// must agree on this shape. That's why it lives in Shared.
public sealed record ShipmentCreatedEvent
{
    // When was this event created?
    public Guid EventId { get; init; } = Guid.NewGuid();
    public DateTime OccurredAt { get; init; } = DateTime.UtcNow;

    // What shipment was created?
    public Guid ShipmentId { get; init; }
    public string TrackingCode { get; init; } = string.Empty;
    public string SenderName { get; init; } = string.Empty;
    public string ReceiverName { get; init; } = string.Empty;
    public string OriginAddress { get; init; } = string.Empty;
    public string DestinationAddress { get; init; } = string.Empty;
}