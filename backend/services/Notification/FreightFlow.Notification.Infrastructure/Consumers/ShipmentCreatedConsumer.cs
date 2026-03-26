using FreightFlow.Notification.Application.Handlers;
using FreightFlow.Notification.Application.Models;
using FreightFlow.Shared.Contracts.Events;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace FreightFlow.Notification.Infrastructure.Consumers;

// IConsumer<T> tells MassTransit: "when ShipmentCreatedEvent arrives,
// call this class". MassTransit handles queue creation, binding,
// acknowledgement, and retries automatically.
public sealed class ShipmentCreatedConsumer : IConsumer<ShipmentCreatedEvent>
{
    private readonly NotificationChainBuilder _chainBuilder;
    private readonly ILogger<ShipmentCreatedConsumer> _logger;

    public ShipmentCreatedConsumer(
        NotificationChainBuilder chainBuilder,
        ILogger<ShipmentCreatedConsumer> logger)
    {
        _chainBuilder = chainBuilder;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<ShipmentCreatedEvent> context)
    {
        var evt = context.Message;

        _logger.LogInformation(
            "📦 ShipmentCreatedEvent received for shipment {TrackingCode}. " +
            "Starting notification chain...",
            evt.TrackingCode);

        // Map the RabbitMQ event to our internal model
        // Why map? Consumer is infrastructure, chain is application.
        // They should not share types — this keeps layers clean.
        var notificationContext = new NotificationContext
        {
            ShipmentId = evt.ShipmentId,
            TrackingCode = evt.TrackingCode,
            SenderName = evt.SenderName,
            ReceiverName = evt.ReceiverName,
            OriginAddress = evt.OriginAddress,
            DestinationAddress = evt.DestinationAddress
        };

        // Run the full notification chain
        await _chainBuilder.ExecuteAsync(notificationContext);

        _logger.LogInformation(
            "✅ Notification chain completed for {TrackingCode}. " +
            "Email={EmailSent}, SMS={SmsSent}, Push={PushSent}",
            evt.TrackingCode,
            notificationContext.EmailSent,
            notificationContext.SmsSent,
            notificationContext.PushSent);
    }
}