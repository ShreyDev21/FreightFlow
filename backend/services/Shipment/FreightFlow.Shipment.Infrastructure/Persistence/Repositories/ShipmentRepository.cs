using FreightFlow.Shipment.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using ShipmentEntity = FreightFlow.Shipment.Domain.Entities.Shipment;

namespace FreightFlow.Shipment.Infrastructure.Persistence.Repositories;

public sealed class ShipmentRepository : IShipmentRepository
{
    private readonly ShipmentDbContext _context;

    public ShipmentRepository(ShipmentDbContext context)
    {
        _context = context;
    }

    public async Task<ShipmentEntity?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _context.Shipments
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<ShipmentEntity>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.Shipments
            .OrderByDescending(s => s.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(
        ShipmentEntity shipment,
        CancellationToken cancellationToken = default)
    {
        await _context.Shipments.AddAsync(shipment, cancellationToken);
    }

    public async Task SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}