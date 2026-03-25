using FreightFlow.Shipment.Application.DTOs;
using FreightFlow.Shipment.Application.Interfaces;
using MediatR;
using ShipmentEntity = FreightFlow.Shipment.Domain.Entities.Shipment;

namespace FreightFlow.Shipment.Application.Commands.CreateShipment;

public sealed class CreateShipmentCommandHandler
    : IRequestHandler<CreateShipmentCommand, ShipmentDto>
{
    private readonly IShipmentRepository _repository;

    public CreateShipmentCommandHandler(IShipmentRepository repository)
    {
        _repository = repository;
    }

    public async Task<ShipmentDto> Handle(
        CreateShipmentCommand request,
        CancellationToken cancellationToken)
    {
        var shipment = ShipmentEntity.Create(
            request.SenderName,
            request.ReceiverName,
            request.OriginAddress,
            request.DestinationAddress,
            request.WeightKg);

        await _repository.AddAsync(shipment, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

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