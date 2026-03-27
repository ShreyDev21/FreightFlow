using MediatR;

namespace FreightFlow.Tracking.Application.Commands.UpdateLocation;

// Driver sends this every 2 seconds with their current GPS position
public sealed record UpdateLocationCommand(
    Guid ShipmentId,
    double Latitude,
    double Longitude) : IRequest;