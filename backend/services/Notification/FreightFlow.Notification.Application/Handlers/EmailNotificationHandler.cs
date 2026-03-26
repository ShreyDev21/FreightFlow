using FreightFlow.Notification.Application.Models;
using Microsoft.Extensions.Logging;

namespace FreightFlow.Notification.Application.Handlers;

public sealed class EmailNotificationHandler : NotificationHandler
{
    private readonly ILogger<EmailNotificationHandler> _logger;

    public EmailNotificationHandler(ILogger<EmailNotificationHandler> logger)
    {
        _logger = logger;
    }

    public override async Task HandleAsync(NotificationContext context)
    {
        // In real world: call SendGrid API here
        // For now we simulate and log

        _logger.LogInformation(
            "📧 EMAIL sent to receiver '{ReceiverName}' for shipment {TrackingCode}. " +
            "Your package from {Origin} to {Destination} is on its way!",
            context.ReceiverName,
            context.TrackingCode,
            context.OriginAddress,
            context.DestinationAddress);

        context.EmailSent = true;

        // Pass to next handler in chain (SMS)
        await base.HandleAsync(context);
    }
}