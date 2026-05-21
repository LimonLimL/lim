using Application.LocationsContext.CreateLocation;
using DirectoryService.Domain.LocationContexts.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace DirectoryService.WebApi.Controllers;

[ApiController]
[Route("api/locations")]
public sealed class LocationsController : ControllerBase
{
    [HttpPost]
    public async Task<IResult> CreateLocation(
        [FromBody] CreateLocationRequest request,
        [FromServices] CreateLocationHandler handler,
        CancellationToken ct
    )
    {
        try
        {
            var command = new CreateLocationCommand(
                request.Name,
                request.Address,
                request.TimeZone
            );

            var createdId = await handler.Handle(command, ct);
            return Results.Created($"/api/locations/{createdId}", new { Id = createdId });
        }
        catch (ArgumentException ex)
        {
            return Results.BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return Results.Conflict(ex.Message);
        }
        catch (Exception ex)
        {
            return Results.Problem(
                $"Детальная ошибка: {ex.Message}\n\n"
                    + $"Stack trace: {ex.StackTrace}\n\n"
                    + $"Inner exception: {ex.InnerException?.Message}"
            );
        }
    }

    [HttpGet]
    public async Task<IResult> GetLocations(
        [FromServices] ILocationRepository repository,
        CancellationToken ct
    )
    {
        var locations = await repository.GetAll(ct);
        return Results.Ok(locations);
    }

    [HttpGet("{id}")]
    public async Task<IResult> GetLocationById(
        [FromRoute] Guid id,
        [FromServices] ILocationRepository repository,
        CancellationToken ct
    )
    {
        var location = await repository.GetById(id, ct);

        if (location is null)
            return Results.NotFound($"Локация с ID {id} не найдена.");

        return Results.Ok(location);
    }

    [HttpPut("{id}")]
    public async Task<IResult> UpdateLocation(
        [FromRoute] Guid id,
        [FromBody] UpdateLocationRequest request,
        [FromServices] UpdateLocationHandler handler,
        CancellationToken ct
    )
    {
        try
        {
            var command = new UpdateLocationCommand(id, request.NewName, request.NewAddress);
            var updatedId = await handler.Handle(command, ct);
            return Results.Ok(new { Id = updatedId });
        }
        catch (ArgumentException ex)
        {
            return Results.BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return Results.Conflict(ex.Message);
        }
        catch (Exception)
        {
            return Results.Problem("Произошла ошибка при обновлении локации.");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IResult> DeleteLocation(
        [FromRoute] Guid id,
        [FromServices] DeleteLocationHandler handler,
        CancellationToken ct
    )
    {
        try
        {
            var command = new DeleteLocationCommand(id);
            var deletedId = await handler.Handle(command, ct);
            return Results.Ok(new { Id = deletedId });
        }
        catch (InvalidOperationException ex)
        {
            return Results.NotFound(ex.Message);
        }
        catch (Exception)
        {
            return Results.Problem("Произошла ошибка при удалении локации.");
        }
    }
}

public sealed record CreateLocationRequest(string Name, string Address, string TimeZone);

public sealed record UpdateLocationRequest(string? NewName, string? NewAddress);
