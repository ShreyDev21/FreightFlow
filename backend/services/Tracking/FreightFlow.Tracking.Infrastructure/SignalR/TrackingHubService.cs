using FreightFlow.Tracking.Application.DTOs;
using FreightFlow.Tracking.Application.Interfaces;
using FreightFlow.Tracking.Infrastructure.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace FreightFlow.Tracking.Infrastructure.SignalR;

public sealed class TrackingHubService : ITrackingHubService
{
    private readonly IHubContext<TrackingHub> _hubContext;

    public TrackingHubService(IHubContext<TrackingHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task BroadcastLocationAsync(
        LocationUpdateDto location,
        CancellationToken cancellationToken = default)
    {
        await _hubContext
            .Clients
            .Group(location.ShipmentId.ToString())
            .SendAsync("LocationUpdated", location, cancellationToken);
    }
}