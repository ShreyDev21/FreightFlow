using FreightFlow.Notification.Application.Models;

namespace FreightFlow.Notification.Application.Handlers;

// Abstract base — defines the chain structure
// Every concrete handler extends this
public abstract class NotificationHandler
{
    // Reference to the next handler in the chain
    private NotificationHandler? _next;

    // Fluent method to wire up the chain:
    // email.SetNext(sms).SetNext(push)
    public NotificationHandler SetNext(NotificationHandler next)
    {
        _next = next;
        return next;
    }

    // Each handler calls this base to pass to next in chain
    public virtual async Task HandleAsync(NotificationContext context)
    {
        if (_next is not null)
            await _next.HandleAsync(context);
    }
}