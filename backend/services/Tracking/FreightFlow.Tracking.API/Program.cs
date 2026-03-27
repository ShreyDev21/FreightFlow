using FreightFlow.Tracking.Infrastructure;
using FreightFlow.Tracking.Infrastructure.Hubs;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTrackingInfrastructure(builder.Configuration);

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(
        typeof(FreightFlow.Tracking.Application.Commands.UpdateLocation
            .UpdateLocationCommand).Assembly));

builder.Services.AddControllers();
builder.Services.AddSignalR();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
    {
        Title = "FreightFlow Tracking API",
        Version = "v1"
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "FreightFlow Tracking API v1");
        c.RoutePrefix = string.Empty;
    });
}

app.MapControllers();

// Hub now lives in Infrastructure namespace
app.MapHub<TrackingHub>("/hubs/tracking");

app.Run();