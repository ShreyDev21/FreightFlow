using FreightFlow.Shipment.Application.Commands.CreateShipment;
using FreightFlow.Shipment.Application.DTOs;
using FreightFlow.Shipment.Application.Queries.GetShipmentById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FreightFlow.Shipment.API.Controllers;

[ApiController]
[Route("api/shipments")]
public sealed class ShipmentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ShipmentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // POST /api/shipments
    [HttpPost]
    public async Task<ActionResult<ShipmentDto>> Create(
        [FromBody] CreateShipmentCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    // GET /api/shipments/{id}
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ShipmentDto>> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetShipmentByIdQuery(id), cancellationToken);

        if (result is null)
            return NotFound();

        return Ok(result);
    }
}
