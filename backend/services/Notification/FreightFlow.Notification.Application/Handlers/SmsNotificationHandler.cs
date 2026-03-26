using FreightFlow.Notification.Application.Models;
using Microsoft.Extensions.Logging;

namespace FreightFlow.Notification.Application.Handlers;

public sealed class SmsNotificationHandler : NotificationHandler
{
    private readonly ILogger<SmsNotificationHandler> _logger;

    public SmsNotificationHandler(ILogger<SmsNotificationHandler> logger)
    {
        _logger = logger;
    }

    public override async Task HandleAsync(NotificationContext context)
    {
        // In real world: call Twilio API here

        _logger.LogInformation(
            "📱 SMS sent to receiver '{ReceiverName}' for shipment {TrackingCode}.",
            context.ReceiverName,
            context.TrackingCode);

        context.SmsSent = true;

        // Pass to next handler in chain (Push)
        await base.HandleAsync(context);
    }
}