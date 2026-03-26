using FreightFlow.Shared.Contracts.Events;
using FreightFlow.Shipment.Application.DTOs;
using FreightFlow.Shipment.Application.Interfaces;
using MassTransit;
using MediatR;
using ShipmentEntity = FreightFlow.Shipment.Domain.Entities.Shipment;

namespace FreightFlow.Shipment.Application.Commands.CreateShipment;

public sealed class CreateShipmentCommandHandler
    : IRequestHandler<CreateShipmentCommand, ShipmentDto>
{
    private readonly IShipmentRepository _repository;
    private readonly IPublishEndpoint _publishEndpoint;

    public CreateShipmentCommandHandler(
        IShipmentRepository repository,
        IPublishEndpoint publishEndpoint)
    {
        _repository = repository;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<ShipmentDto> Handle(
        CreateShipmentCommand request,
        CancellationToken cancellationToken)
    {
        // Step 1 — Create domain entity (business rules enforced here)
        var shipment = ShipmentEntity.Create(
            request.SenderName,
            request.ReceiverName,
            request.OriginAddress,
            request.DestinationAddress,
            request.WeightKg);

        // Step 2 — Persist to NeonDB
        await _repository.AddAsync(shipment, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        // Step 3 — Publish event to RabbitMQ
        // Why publish AFTER saving? If DB save fails, no event is sent.
        // If publish fails, shipment is still saved (we can retry later).
        // This order prevents ghost events for failed shipments.
        await _publishEndpoint.Publish(new ShipmentCreatedEvent
        {
            ShipmentId = shipment.Id,
            TrackingCode = shipment.TrackingCode.ToString(),
            SenderName = shipment.SenderName,
            ReceiverName = shipment.ReceiverName,
            OriginAddress = shipment.OriginAddress,
            DestinationAddress = shipment.DestinationAddress
        }, cancellationToken);

        // Step 4 — Return DTO to API layer
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