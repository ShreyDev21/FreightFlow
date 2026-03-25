using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShipmentEntity = FreightFlow.Shipment.Domain.Entities.Shipment;

namespace FreightFlow.Shipment.Infrastructure.Persistence.Configurations;

public sealed class ShipmentConfiguration : IEntityTypeConfiguration<ShipmentEntity>
{
    public void Configure(EntityTypeBuilder<ShipmentEntity> builder)
    {
        builder.ToTable("shipments");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id)
            .HasColumnName("id");

        // Map TrackingCode value object to a plain string column
        builder.Property(s => s.TrackingCode)
            .HasColumnName("tracking_code")
            .HasMaxLength(50)
            .HasConversion(
                tc => tc.ToString(),
                value => FreightFlow.Shipment.Domain.ValueObjects.TrackingCode.From(value))
            .IsRequired();

        builder.HasIndex(s => s.TrackingCode)
            .IsUnique();

        builder.Property(s => s.SenderName)
            .HasColumnName("sender_name")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(s => s.ReceiverName)
            .HasColumnName("receiver_name")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(s => s.OriginAddress)
            .HasColumnName("origin_address")
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(s => s.DestinationAddress)
            .HasColumnName("destination_address")
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(s => s.WeightKg)
            .HasColumnName("weight_kg")
            .HasPrecision(10, 2)
            .IsRequired();

        builder.Property(s => s.Status)
            .HasColumnName("status")
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(s => s.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(s => s.UpdatedAt)
            .HasColumnName("updated_at");
    }
}