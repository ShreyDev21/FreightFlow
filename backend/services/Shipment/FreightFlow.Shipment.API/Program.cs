using FreightFlow.Shipment.Infrastructure;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

// Infrastructure (DbContext, Repository)
builder.Services.AddInfrastructure(builder.Configuration);

// MediatR — scans Application assembly for all handlers
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(
        typeof(FreightFlow.Shipment.Application.Commands.CreateShipment.CreateShipmentCommand).Assembly));

// Controllers
builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();

app.Run();
