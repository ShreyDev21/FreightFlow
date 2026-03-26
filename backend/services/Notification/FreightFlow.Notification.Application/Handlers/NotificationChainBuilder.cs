using FreightFlow.Notification.Application.Models;
using Microsoft.Extensions.Logging;

namespace FreightFlow.Notification.Application.Handlers;

public sealed class NotificationChainBuilder
{
    private readonly ILogger<EmailNotificationHandler> _emailLogger;
    private readonly ILogger<SmsNotificationHandler> _smsLogger;
    private readonly ILogger<PushNotificationHandler> _pushLogger;

    public NotificationChainBuilder(
        ILogger<EmailNotificationHandler> emailLogger,
        ILogger<SmsNotificationHandler> smsLogger,
        ILogger<PushNotificationHandler> pushLogger)
    {
        _emailLogger = emailLogger;
        _smsLogger = smsLogger;
        _pushLogger = pushLogger;
    }

    // Builds and returns the first handler in the chain
    // Chain order: Email → SMS → Push
    public NotificationHandler Build()
    {
        var email = new EmailNotificationHandler(_emailLogger);
        var sms = new SmsNotificationHandler(_smsLogger);
        var push = new PushNotificationHandler(_pushLogger);

        // Wire the chain: email → sms → push
        email.SetNext(sms).SetNext(push);

        // Return the first handler — caller just calls HandleAsync()
        // and the chain runs automatically
        return email;
    }

    // Run the full chain for a notification context
    public async Task ExecuteAsync(NotificationContext context)
    {
        var chain = Build();
        await chain.HandleAsync(context);
    }
}