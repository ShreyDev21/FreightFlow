using FreightFlow.Notification.Application.Models;
using Microsoft.Extensions.Logging;

namespace FreightFlow.Notification.Application.Handlers;

public sealed class PushNotificationHandler : NotificationHandler
{
    private readonly ILogger<PushNotificationHandler> _logger;

    public PushNotificationHandler(ILogger<PushNotificationHandler> logger)
    {
        _logger = logger;
    }

    public override async Task HandleAsync(NotificationContext context)
    {
        // In real world: call Firebase/APNs here

        _logger.LogInformation(
            "🔔 PUSH notification sent for shipment {TrackingCode}. " +
            "Email={EmailSent}, SMS={SmsSent}",
            context.TrackingCode,
            context.EmailSent,
            context.SmsSent);

        context.PushSent = true;

        // End of chain — no next handler
        await base.HandleAsync(context);
    }
}