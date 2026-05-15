using DirectoryService.Domain.PositionContext;
using DirectoryService.Domain.PositionContext.ValueObjects;
using DirectoryService.Domain.Shared.ValueObjects;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Storage;

namespace DirectoryService.WebApi.Controllers;

[ApiController]
[Route("api/positions")]
public sealed class PositionsController : ControllerBase
{
    [HttpGet]
    public IResult GetPositions()
    {
        var positions = Storage.GetAllPositions();
        return Results.Ok(positions);
    }

    [HttpGet("{id}")]
    public IResult GetPositionById([FromRoute(Name = "id")] Guid id)
    {
        var positionId = PositionId.From(id);
        var position = Storage.GetById(positionId);

        if (position is null)
            return Results.NotFound($"Должность с ID {id} не найдена или архивирована.");

        return Results.Ok(position);
    }

    [HttpPost]
    public IResult CreatePosition([FromBody] CreatePositionRequest request)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                return Results.BadRequest("Название должности не может быть пустым.");

            var name = PositionName.Create(request.Name);
            var description = PositionDescription.Create(request.Description ?? string.Empty);
            var isActive = IsActive.Create(request.IsActive);
            var now = DateTime.UtcNow;
            var lifeTime = EntityLifeTime.Create(now, now, null, true);

            var existingPositions = Storage.GetAllPositions().ToList();
            var verification = new InMemoryPosVerification(existingPositions);

            var position = Position.Create(name, description, isActive, lifeTime, verification);

            Storage.Add(position);

            return Results.Created($"/api/positions/{position.Id.Value}", position);
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
            return Results.Problem("Произошла ошибка при создании должности.");
        }
    }

    [HttpPatch("{id}")]
    public IResult UpdatePosition(
        [FromRoute(Name = "id")] Guid id,
        [FromBody] UpdatePositionRequest request
    )
    {
        try
        {
            var positionId = PositionId.From(id);
            var existingPosition = Storage.GetById(positionId);

            if (existingPosition is null)
                return Results.NotFound($"Должность с ID {id} не найдена или архивирована.");

            if (request.Name is null && request.Description is null && request.IsActive is null)
                return Results.BadRequest("Хотя бы одно поле для обновления должно быть указано.");

            if (request.Name is not null && string.IsNullOrWhiteSpace(request.Name))
                return Results.BadRequest("Название должности не может быть пустым.");

            if (
                request.Name is not null
                && !request.Name.Equals(
                    existingPosition.Name.Value,
                    StringComparison.OrdinalIgnoreCase
                )
            )
            {
                var allPositions = Storage.GetAllPositions();
                if (
                    allPositions.Any(p =>
                        p.Name.Value.Equals(request.Name, StringComparison.OrdinalIgnoreCase)
                    )
                )
                    return Results.Conflict($"Должность с именем '{request.Name}' уже существует.");
            }

            var newName = request.Name is not null
                ? PositionName.Create(request.Name)
                : existingPosition.Name;
            var newDescription = request.Description is not null
                ? PositionDescription.Create(request.Description)
                : existingPosition.Description;
            var newIsActive = request.IsActive.HasValue
                ? IsActive.Create(request.IsActive.Value)
                : existingPosition.IsActive;
            var newLifeTime = existingPosition.LifeTime.Update();

            var updatedPosition = Position.Restore(
                existingPosition.Id,
                newName,
                newDescription,
                newIsActive,
                newLifeTime
            );

            Storage.HardRemove(positionId);
            Storage.Add(updatedPosition);

            return Results.Ok(updatedPosition);
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
            return Results.Problem("Произошла ошибка при обновлении должности.");
        }
    }

    [HttpDelete("{id}")]
    public IResult DeletePosition([FromRoute(Name = "id")] Guid id)
    {
        try
        {
            var positionId = PositionId.From(id);
            var existingPosition = Storage.GetById(positionId);

            if (existingPosition is null)
                return Results.NotFound($"Должность с ID {id} не найдена или уже архивирована.");

            Storage.Remove(positionId);
            return Results.Ok($"Должность с ID {id} успешно архивирована.");
        }
        catch (KeyNotFoundException ex)
        {
            return Results.NotFound(ex.Message);
        }
        catch (Exception)
        {
            return Results.Problem("Произошла ошибка при удалении должности.");
        }
    }

    [HttpDelete("{id}/hard")]
    public IResult HardDeletePosition([FromRoute(Name = "id")] Guid id)
    {
        try
        {
            var positionId = PositionId.From(id);
            var removed = Storage.HardRemove(positionId);

            if (!removed)
                return Results.NotFound($"Должность с ID {id} не найдена.");

            return Results.Ok($"Должность с ID {id} успешно удалена.");
        }
        catch (Exception)
        {
            return Results.Problem("Произошла ошибка при удалении должности.");
        }
    }
}

public sealed record CreatePositionRequest(string Name, string Description, bool IsActive);

public sealed record UpdatePositionRequest(
    string? Name = null,
    string? Description = null,
    bool? IsActive = null
);
