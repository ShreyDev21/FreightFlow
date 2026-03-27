using Microsoft.AspNetCore.SignalR;

namespace FreightFlow.Tracking.Infrastructure.Hubs;

public sealed class TrackingHub : Hub
{
    public async Task WatchShipment(string shipmentId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, shipmentId);
    }

    public async Task UnwatchShipment(string shipmentId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, shipmentId);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await base.OnDisconnectedAsync(exception);
    }
}