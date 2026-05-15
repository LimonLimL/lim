using DirectoryService.Domain.LocationsContext;
using DirectoryService.Domain.LocationsContext.ValueObjects;
using DirectoryService.Domain.Shared.ValueObjects;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Storage;

namespace DirectoryService.WebApi.Controllers;

[ApiController]
[Route("api/locations")]
public sealed class LocationsController : ControllerBase
{
    [HttpGet]
    public IResult GetLocations()
    {
        var locations = Storage.GetAllLocations();
        return Results.Ok(locations);
    }

    [HttpGet("{id}")]
    public IResult GetLocationById([FromRoute(Name = "id")] Guid id)
    {
        var locationId = LocationId.From(id);
        var location = Storage.GetById(locationId);

        if (location is null)
            return Results.NotFound($"Локация с ID {id} не найдена или архивирована.");

        return Results.Ok(location);
    }

    [HttpPost]
    public IResult CreateLocation([FromBody] CreateLocationRequest request)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                return Results.BadRequest("Название локации не может быть пустым.");

            if (string.IsNullOrWhiteSpace(request.Address))
                return Results.BadRequest("Адрес локации не может быть пустым.");

            if (string.IsNullOrWhiteSpace(request.TimeZone))
                return Results.BadRequest("Часовой пояс не может быть пустым.");

            var now = DateTime.UtcNow;
            var location = Location.Create(
                Guid.NewGuid(),
                request.Address,
                request.Name,
                request.TimeZone,
                now,
                now
            );

            Storage.Add(location);

            return Results.Created($"/api/locations/{location.Id.Value}", location);
        }
        catch (InvalidOperationException ex)
        {
            return Results.Conflict(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return Results.BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return Results.Problem("Произошла ошибка при создании локации.");
        }
    }

    [HttpPatch("{id}")]
    public IResult UpdateLocation(
        [FromRoute(Name = "id")] Guid id,
        [FromBody] UpdateLocationRequest request
    )
    {
        try
        {
            var locationId = LocationId.From(id);
            var existingLocation = Storage.GetById(locationId);

            if (existingLocation is null)
                return Results.NotFound($"Локация с ID {id} не найдена или архивирована.");

            if (request.Name is null && request.Address is null && request.TimeZone is null)
                return Results.BadRequest("Хотя бы одно поле для обновления должно быть указано.");

            if (request.Name is not null && string.IsNullOrWhiteSpace(request.Name))
                return Results.BadRequest("Название локации не может быть пустым.");

            if (request.Address is not null && string.IsNullOrWhiteSpace(request.Address))
                return Results.BadRequest("Адрес локации не может быть пустым.");

            if (request.TimeZone is not null && string.IsNullOrWhiteSpace(request.TimeZone))
                return Results.BadRequest("Часовой пояс не может быть пустым.");

            if (
                request.Name is not null
                && !request.Name.Equals(
                    existingLocation.Name.Value,
                    StringComparison.OrdinalIgnoreCase
                )
            )
            {
                var allLocations = Storage.GetAllLocations();
                if (
                    allLocations.Any(l =>
                        l.Name.Value.Equals(request.Name, StringComparison.OrdinalIgnoreCase)
                    )
                )
                    return Results.Conflict($"Локация с именем '{request.Name}' уже существует.");
            }

            var now = DateTime.UtcNow;
            var updatedLocation = new Location(
                existingLocation.Id,
                request.Address is not null
                    ? LocationAddress.Create(request.Address)
                    : existingLocation.Address,
                request.Name is not null
                    ? LocationName.Create(request.Name)
                    : existingLocation.Name,
                request.TimeZone is not null
                    ? IanaTimeZone.Create(request.TimeZone)
                    : existingLocation.TimeZone,
                existingLocation.LifeTime.Update()
            );

            Storage.HardRemove(locationId);
            Storage.Add(updatedLocation);

            return Results.Ok(updatedLocation);
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
    public IResult DeleteLocation([FromRoute(Name = "id")] Guid id)
    {
        try
        {
            var locationId = LocationId.From(id);
            var existingLocation = Storage.GetById(locationId);

            if (existingLocation is null)
                return Results.NotFound($"Локация с ID {id} не найдена или уже архивирована.");

            Storage.Remove(locationId);
            return Results.Ok($"Локация с ID {id} успешно архивирована.");
        }
        catch (KeyNotFoundException ex)
        {
            return Results.NotFound(ex.Message);
        }
        catch (Exception)
        {
            return Results.Problem("Произошла ошибка при удалении локации.");
        }
    }

    [HttpDelete("{id}/hard")]
    public IResult HardDeleteLocation([FromRoute(Name = "id")] Guid id)
    {
        try
        {
            var locationId = LocationId.From(id);
            var removed = Storage.HardRemove(locationId);

            if (!removed)
                return Results.NotFound($"Локация с ID {id} не найдена.");

            return Results.Ok($"Локация с ID {id} успешно удалена.");
        }
        catch (Exception)
        {
            return Results.Problem("Произошла ошибка при удалении локации.");
        }
    }
}

public sealed record CreateLocationRequest(string Address, string Name, string TimeZone);

public sealed record UpdateLocationRequest(
    string? Address = null,
    string? Name = null,
    string? TimeZone = null
);
