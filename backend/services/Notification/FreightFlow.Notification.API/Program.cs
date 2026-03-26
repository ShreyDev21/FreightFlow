using FreightFlow.Notification.Infrastructure;

var builder = Host.CreateApplicationBuilder(args);

// Wire up MassTransit consumer + chain handlers
builder.Services.AddNotificationInfrastructure(builder.Configuration);

var app = builder.Build();

app.Run();