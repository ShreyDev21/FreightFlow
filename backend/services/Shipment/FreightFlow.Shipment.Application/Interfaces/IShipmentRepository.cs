using ShipmentEntity = FreightFlow.Shipment.Domain.Entities.Shipment;

namespace FreightFlow.Shipment.Application.Interfaces;

public interface IShipmentRepository
{
    Task<ShipmentEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<ShipmentEntity>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(ShipmentEntity shipment, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}