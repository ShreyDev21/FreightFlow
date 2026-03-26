namespace FreightFlow.Notification.Application.Models;

// This is the internal model that flows through the chain.
// It's completely decoupled from the RabbitMQ message shape.
public sealed class NotificationContext
{
    public Guid ShipmentId { get; init; }
    public string TrackingCode { get; init; } = string.Empty;
    public string ReceiverName { get; init; } = string.Empty;
    public string SenderName { get; init; } = string.Empty;
    public string OriginAddress { get; init; } = string.Empty;
    public string DestinationAddress { get; init; } = string.Empty;

    // These track what happened as the chain runs
    public bool EmailSent { get; set; }
    public bool SmsSent { get; set; }
    public bool PushSent { get; set; }
}