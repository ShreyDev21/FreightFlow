using FreightFlow.Shipment.Application.DTOs;
using MediatR;

namespace FreightFlow.Shipment.Application.Queries.GetShipmentById;

// IRequest<ShipmentDto?> — nullable because shipment might not exist
public sealed record GetShipmentByIdQuery(Guid Id) : IRequest<ShipmentDto?>;