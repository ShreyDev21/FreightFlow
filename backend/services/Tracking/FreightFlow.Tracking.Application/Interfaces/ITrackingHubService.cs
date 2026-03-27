using FreightFlow.Tracking.Application.DTOs;

namespace FreightFlow.Tracking.Application.Interfaces;

// Contract for broadcasting location updates to connected browsers
// Application defines it, Infrastructure (SignalR) implements it
public interface ITrackingHubService
{
    Task BroadcastLocationAsync(LocationUpdateDto location, CancellationToken cancellationToken = default);
}