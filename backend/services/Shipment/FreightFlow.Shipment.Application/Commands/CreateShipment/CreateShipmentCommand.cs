using FreightFlow.Shipment.Application.DTOs;
using MediatR;

namespace FreightFlow.Shipment.Application.Commands.CreateShipment;

// IRequest<ShipmentDto> means: this command returns a ShipmentDto when handled
public sealed record CreateShipmentCommand(
    string SenderName,
    string ReceiverName,
    string OriginAddress,
    string DestinationAddress,
    decimal WeightKg) : IRequest<ShipmentDto>;