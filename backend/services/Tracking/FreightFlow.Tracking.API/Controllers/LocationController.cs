using FreightFlow.Tracking.Application.Commands.UpdateLocation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FreightFlow.Tracking.API.Controllers;

[ApiController]
[Route("api/tracking")]
public sealed class LocationController : ControllerBase
{
    private readonly IMediator _mediator;

    public LocationController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // POST /api/tracking/location
    // Driver pushes GPS here every 2 seconds
    [HttpPost("location")]
    public async Task<IActionResult> UpdateLocation(
        [FromBody] UpdateLocationCommand command,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(command, cancellationToken);

        // 204 No Content — location saved and broadcast,
        // nothing to return to the driver
        return NoContent();
    }
}