namespace FreightFlow.Shipment.Domain.Enums;

public enum ShipmentStatus
{
    Pending = 1,
    Confirmed = 2,
    PickedUp = 3,
    InTransit = 4,
    OutForDelivery = 5,
    Delivered = 6,
    Cancelled = 7,
    Failed = 8
}