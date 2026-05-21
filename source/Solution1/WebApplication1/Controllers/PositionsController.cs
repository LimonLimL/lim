using DirectoryService.Domain.PositionContexts.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace DirectoryService.WebApi.Controllers;

[ApiController]
[Route("api/positions")]
public sealed class PositionsController : ControllerBase
{
    [HttpPost]
    public async Task<IResult> CreatePosition(
        [FromBody] CreatePositionRequest request,
        [FromServices] CreatePositionHandler handler,
        CancellationToken ct
    )
    {
        try
        {
            var command = new CreatePositionCommand(request.Name, request.Description);

            var createdId = await handler.Handle(command, ct);
            return Results.Created($"/api/positions/{createdId}", new { Id = createdId });
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
            return Results.Problem("Произошла ошибка при создании должности.");
        }
    }

    [HttpGet]
    public async Task<IResult> GetPositions(
        [FromServices] IPositionRepository repository,
        CancellationToken ct
    )
    {
        var positions = await repository.GetAll(ct);
        return Results.Ok(positions);
    }

    [HttpGet("{id}")]
    public async Task<IResult> GetPositionById(
        [FromRoute] Guid id,
        [FromServices] IPositionRepository repository,
        CancellationToken ct
    )
    {
        var position = await repository.GetById(id, ct);

        if (position is null)
            return Results.NotFound($"Должность с ID {id} не найдена.");

        return Results.Ok(position);
    }

    [HttpPut("{id}/rename")]
    public async Task<IResult> RenamePosition(
        [FromRoute] Guid id,
        [FromBody] RenamePositionRequest request,
        [FromServices] RenamePositionHandler handler,
        CancellationToken ct
    )
    {
        try
        {
            var command = new RenamePositionCommand(id, request.NewName);
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
            return Results.Problem("Произошла ошибка при переименовании должности.");
        }
    }

    [HttpDelete]
    public async Task<IResult> DeletePositions(
        [FromBody] DeletePositionsRequest request,
        [FromServices] DeletePositionHandler handler,
        CancellationToken ct
    )
    {
        try
        {
            var command = new DeletePositionCommand(request.Ids);
            var deletedIds = await handler.Handle(command, ct);
            return Results.Ok(new { Ids = deletedIds });
        }
        catch (InvalidOperationException ex)
        {
            return Results.NotFound(ex.Message);
        }
        catch (Exception)
        {
            return Results.Problem("Произошла ошибка при удалении должностей.");
        }
    }
}

public sealed record CreatePositionRequest(string Name, string? Description = null);

public sealed record RenamePositionRequest(string NewName);

public sealed record DeletePositionsRequest(IEnumerable<Guid> Ids);
