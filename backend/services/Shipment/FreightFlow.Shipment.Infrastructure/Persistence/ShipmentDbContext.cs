using Microsoft.EntityFrameworkCore;
using ShipmentEntity = FreightFlow.Shipment.Domain.Entities.Shipment;

namespace FreightFlow.Shipment.Infrastructure.Persistence;

public sealed class ShipmentDbContext : DbContext
{
    public ShipmentDbContext(DbContextOptions<ShipmentDbContext> options)
        : base(options) { }

    public DbSet<ShipmentEntity> Shipments => Set<ShipmentEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Auto-apply all IEntityTypeConfiguration classes in this assembly
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ShipmentDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}