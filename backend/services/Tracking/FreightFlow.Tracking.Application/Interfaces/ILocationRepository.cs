using FreightFlow.Tracking.Domain.Entities;

namespace FreightFlow.Tracking.Application.Interfaces;

public interface ILocationRepository
{
    Task SaveAsync(LocationUpdate location, CancellationToken cancellationToken = default);
    Task<LocationUpdate?> GetLatestAsync(Guid shipmentId, CancellationToken cancellationToken = default);
}