using Application.DepartmentContext.CreateDepartment;
using Application.DepartmentContext.UpdateDepartment;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/departments")]
public sealed class DepartmentsController : ControllerBase
{
    [HttpPost]
    public async Task<IResult> CreateDepartment(
        [FromBody] CreateDepartmentRequest request,
        [FromServices] CreateDepartmentHandler handler,
        CancellationToken ct
    )
    {
        try
        {
            var command = new CreateDepartmentCommand(request.Name, request.ParentId);

            var createdId = await handler.Handle(command, ct);
            return Results.Created($"/api/departments/{createdId}", new { Id = createdId });
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
            return Results.Problem("Ошибка при создании отдела.");
        }
    }

    [HttpGet]
    public async Task<IResult> GetDepartments(
        [FromServices] IDepartmentRepository repository,
        CancellationToken ct
    )
    {
        var departments = await repository.GetAll(ct);
        return Results.Ok(departments);
    }

    [HttpGet("{id}")]
    public async Task<IResult> GetDepartmentById(
        [FromRoute] Guid id,
        [FromServices] IDepartmentRepository repository,
        CancellationToken ct
    )
    {
        var department = await repository.GetById(id, ct);

        if (department is null)
            return Results.NotFound($"Отдел с ID {id} не найден.");

        return Results.Ok(department);
    }

    [HttpPut("{id}")]
    public async Task<IResult> UpdateDepartment(
        [FromRoute] Guid id,
        [FromBody] UpdateDepartmentRequest request,
        [FromServices] UpdateDepartmentHandler handler,
        CancellationToken ct
    )
    {
        try
        {
            var command = new UpdateDepartmentCommand(id, request.NewName, request.NewParentId);

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
            return Results.Problem("Ошибка при обновлении отдела.");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IResult> DeleteDepartment(
        [FromRoute] Guid id,
        [FromServices] DeleteDepartmentHandler handler,
        CancellationToken ct
    )
    {
        try
        {
            var command = new DeleteDepartmentCommand(id);
            var deletedId = await handler.Handle(command, ct);
            return Results.Ok(new { Id = deletedId });
        }
        catch (InvalidOperationException ex)
        {
            return Results.NotFound(ex.Message);
        }
        catch (Exception)
        {
            return Results.Problem("Произошла ошибка при удалении отдела.");
        }
    }
}

public sealed record CreateDepartmentRequest(string Name, Guid? ParentId = null);

public sealed record UpdateDepartmentRequest(string? NewName, Guid? NewParentId);
