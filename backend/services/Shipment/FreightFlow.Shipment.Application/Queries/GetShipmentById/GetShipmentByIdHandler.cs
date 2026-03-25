using FreightFlow.Shipment.Application.DTOs;
using FreightFlow.Shipment.Application.Interfaces;
using MediatR;

namespace FreightFlow.Shipment.Application.Queries.GetShipmentById;

public sealed class GetShipmentByIdHandler
    : IRequestHandler<GetShipmentByIdQuery, ShipmentDto?>
{
    private readonly IShipmentRepository _repository;

    public GetShipmentByIdHandler(IShipmentRepository repository)
    {
        _repository = repository;
    }

    public async Task<ShipmentDto?> Handle(
        GetShipmentByIdQuery request,
        CancellationToken cancellationToken)
    {
        var shipment = await _repository.GetByIdAsync(request.Id, cancellationToken);

        // Return null if not found — API layer will convert this to 404
        if (shipment is null)
            return null;

        return new ShipmentDto
        {
            Id = shipment.Id,
            TrackingCode = shipment.TrackingCode.ToString(),
            SenderName = shipment.SenderName,
            ReceiverName = shipment.ReceiverName,
            OriginAddress = shipment.OriginAddress,
            DestinationAddress = shipment.DestinationAddress,
            WeightKg = shipment.WeightKg,
            Status = shipment.Status.ToString(),
            CreatedAt = shipment.CreatedAt,
            UpdatedAt = shipment.UpdatedAt
        };
    }
}