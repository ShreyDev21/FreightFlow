using FreightFlow.Tracking.Application.DTOs;
using FreightFlow.Tracking.Application.Interfaces;
using FreightFlow.Tracking.Domain.Entities;
using MediatR;

namespace FreightFlow.Tracking.Application.Commands.UpdateLocation;

public sealed class UpdateLocationHandler : IRequestHandler<UpdateLocationCommand>
{
    private readonly ILocationRepository _repository;
    private readonly ITrackingHubService _hubService;

    public UpdateLocationHandler(
        ILocationRepository repository,
        ITrackingHubService hubService)
    {
        _repository = repository;
        _hubService = hubService;
    }

    public async Task Handle(
        UpdateLocationCommand request,
        CancellationToken cancellationToken)
    {
        // Step 1 — Create and validate the domain entity
        var location = LocationUpdate.Create(
            request.ShipmentId,
            request.Latitude,
            request.Longitude);

        // Step 2 — Persist the location
        await _repository.SaveAsync(location, cancellationToken);

        // Step 3 — Broadcast to all browsers watching this shipment
        // This is where SignalR fires — every connected browser
        // watching this shipmentId gets the update instantly
        await _hubService.BroadcastLocationAsync(new LocationUpdateDto
        {
            ShipmentId = location.ShipmentId,
            Latitude = location.Latitude,
            Longitude = location.Longitude,
            RecordedAt = location.RecordedAt
        }, cancellationToken);
    }
}