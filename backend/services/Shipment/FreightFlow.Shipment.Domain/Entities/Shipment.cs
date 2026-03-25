using FreightFlow.Shipment.Domain.Enums;
using FreightFlow.Shipment.Domain.ValueObjects;

namespace FreightFlow.Shipment.Domain.Entities;

public sealed class Shipment
{
    public Guid Id { get; private set; }
    public TrackingCode TrackingCode { get; private set; }
    public string SenderName { get; private set; }
    public string ReceiverName { get; private set; }
    public string OriginAddress { get; private set; }
    public string DestinationAddress { get; private set; }
    public decimal WeightKg { get; private set; }
    public ShipmentStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    // EF Core needs a parameterless constructor
    // Properties are initialized by EF Core via reflection
    #pragma warning disable CS8618
    private Shipment() { }
    #pragma warning restore CS8618

    // Factory method — only way to create a valid Shipment
    public static Shipment Create(
        string senderName,
        string receiverName,
        string originAddress,
        string destinationAddress,
        decimal weightKg)
    {
        if (weightKg <= 0)
            throw new ArgumentException("Weight must be greater than zero.", nameof(weightKg));

        return new Shipment
        {
            Id = Guid.NewGuid(),
            TrackingCode = TrackingCode.Generate(),
            SenderName = senderName,
            ReceiverName = receiverName,
            OriginAddress = originAddress,
            DestinationAddress = destinationAddress,
            WeightKg = weightKg,
            Status = ShipmentStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };
    }

    // Business rule: only certain status transitions are allowed
    public void UpdateStatus(ShipmentStatus newStatus)
    {
        if (Status == ShipmentStatus.Delivered)
            throw new InvalidOperationException("Cannot change status of a delivered shipment.");

        if (Status == ShipmentStatus.Cancelled)
            throw new InvalidOperationException("Cannot change status of a cancelled shipment.");

        Status = newStatus;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Cancel()
    {
        if (Status == ShipmentStatus.Delivered)
            throw new InvalidOperationException("Cannot cancel a delivered shipment.");

        if (Status == ShipmentStatus.InTransit)
            throw new InvalidOperationException("Cannot cancel a shipment that is already in transit.");

        Status = ShipmentStatus.Cancelled;
        UpdatedAt = DateTime.UtcNow;
    }
}