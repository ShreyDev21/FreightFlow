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

// Swagger — only in development
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
    {
        Title = "FreightFlow Shipment API",
        Version = "v1",
        Description = "Shipment management service"
    });
});

var app = builder.Build();

// Swagger UI
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "FreightFlow Shipment API v1");
        c.RoutePrefix = string.Empty; // Swagger at root http://localhost:5001
    });
}

app.MapControllers();

app.Run();