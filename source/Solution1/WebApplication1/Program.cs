using DirectoryService.Domain.LocationsContext;
using DirectoryService.Domain.LocationsContext.ValueObjects;
using DirectoryService.Domain.PositionContext;
using DirectoryService.Domain.PositionContext.ValueObjects;
using DirectoryService.Domain.Shared.ValueObjects;
using DirectoryService.WebApi;
using DirectoryService.WebApi.Storages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi;

Storage.InitializeStorage();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc(
        "v1",
        new OpenApiInfo
        {
            Title = "Directory Service API",
            Version = "v1",
            Description = "API для тестирования backend логики",
        }
    );
});

builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Directory Service API V1");
        c.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.MapGet(
    "api/locations",
    () =>
    {
        var locations = Storage.GetAllLocations();
        return Results.Ok(locations);
    }
);
app.MapGet(
    "api/locations/{id}",
    (Guid id) =>
    {
        var locationId = LocationId.From(id);
        var location = Storage.GetById(locationId);

        return location is null
            ? Results.NotFound($"Локация с ID {id} не найдена или архивирована.")
            : Results.Ok(location);
    }
);

app.MapGet(
    "api/positions",
    () =>
    {
        var positions = Storage.GetAllPositions();
        return Results.Ok(positions);
    }
);

app.MapGet(
    "api/positions/{id}",
    (Guid id) =>
    {
        var positionId = PositionId.From(id);
        var position = Storage.GetById(positionId);

        if (position is null)
        {
            return Results.NotFound($"Должность с ID {id} не найдена");
        }

        return Results.Ok(position);
    }
);
app.MapGet(
    "/api/debug/storage",
    () =>
    {
        var locations = Storage.GetAllLocations().ToList();
        var positions = Storage.GetAllPositions().ToList();

        return new
        {
            LocationsCount = locations.Count,
            LocationsIds = locations.Select(l => l.Id.Value).ToList(),
            PositionsCount = positions.Count,
            PositionsIds = positions.Select(p => p.Id.Value).ToList(),
        };
    }
);

app.MapPost(
    "api/locations",
    ([FromBody] CreateLocationRequest request) =>
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
            return Results.Problem("Произошла внутренняя ошибка при создании локации.");
        }
    }
);

app.MapPost(
    "api/positions",
    ([FromBody] CreatePositionRequest request) =>
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                return Results.BadRequest("Название должности не может быть пустым.");

            Console.WriteLine($"[DEBUG] Начинаем создание должности: {request.Name}");

            PositionName name;
            try
            {
                name = PositionName.Create(request.Name);
                Console.WriteLine($"[DEBUG] PositionName создан: {name.Value}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Ошибка при создании PositionName: {ex.Message}");
                return Results.BadRequest($"Ошибка названия: {ex.Message}");
            }

            PositionDescription description;
            try
            {
                description = PositionDescription.Create(request.Description ?? string.Empty);
                Console.WriteLine($"[DEBUG] PositionDescription создан");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Ошибка при создании PositionDescription: {ex.Message}");
                return Results.BadRequest($"Ошибка описания: {ex.Message}");
            }

            IsActive isActive;
            try
            {
                isActive = IsActive.Create(request.IsActive);
                Console.WriteLine($"[DEBUG] IsActive создан: {isActive.Value}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Ошибка при создании IsActive: {ex.Message}");
                return Results.BadRequest($"Ошибка isActive: {ex.Message}");
            }

            EntityLifeTime lifeTime;
            try
            {
                var now = DateTime.UtcNow;
                lifeTime = EntityLifeTime.Create(now, now, true);
                Console.WriteLine($"[DEBUG] EntityLifeTime создан");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Ошибка при создании EntityLifeTime: {ex.Message}");
                return Results.BadRequest($"Ошибка lifeTime: {ex.Message}");
            }

            Console.WriteLine($"[DEBUG] Создаем верификацию...");
            var existingPositions = Storage.GetAllPositions().ToList();
            Console.WriteLine($"[DEBUG] Найдено существующих позиций: {existingPositions.Count}");

            var verification = new InMemoryPosVerification(existingPositions);

            Console.WriteLine($"[DEBUG] Вызываем Position.Create...");
            Position position;
            try
            {
                position = Position.Create(name, description, isActive, lifeTime, verification);
                Console.WriteLine($"[DEBUG] Position создан с ID: {position.Id.Value}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Ошибка при создании Position: {ex.Message}");
                Console.WriteLine($"[ERROR] StackTrace: {ex.StackTrace}");
                return Results.BadRequest($"Ошибка создания: {ex.Message}");
            }

            Console.WriteLine($"[DEBUG] Добавляем в Storage...");
            try
            {
                Storage.Add(position);
                Console.WriteLine($"[DEBUG] Успешно добавлено в Storage");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Ошибка при добавлении в Storage: {ex.Message}");
                Console.WriteLine($"[ERROR] StackTrace: {ex.StackTrace}");
                return Results.Conflict($"Конфликт: {ex.Message}");
            }

            Console.WriteLine($"[SUCCESS] Должность успешно создана!");
            return Results.Created($"/api/positions/{position.Id.Value}", position);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[FATAL ERROR] {ex.Message}");
            Console.WriteLine($"[FATAL ERROR] StackTrace: {ex.StackTrace}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"[FATAL ERROR] InnerException: {ex.InnerException.Message}");
            }
            return Results.Problem($"Произошла внутренняя ошибка: {ex.Message}");
        }
    }
);
app.MapPatch(
    "api/locations/{id}",
    ([FromRoute] Guid id, [FromBody] UpdateLocationRequest request) =>
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
);

app.MapPatch(
    "api/positions/{id}",
    ([FromRoute] Guid id, [FromBody] UpdatePositionRequest request) =>
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
);

app.MapDelete(
    "api/locations/{id}",
    ([FromRoute] Guid id) =>
    {
        try
        {
            var locationId = LocationId.From(id);

            var existingLocation = Storage.GetById(locationId);

            if (existingLocation is null)
                return Results.NotFound($"Локация с ID {id} не найдена или архивирована.");

            Storage.Remove(locationId);

            return Results.NoContent();
        }
        catch (Exception)
        {
            return Results.Problem("Ошибка при удалении локации.");
        }
    }
);

app.MapDelete(
    "api/locations/{id}/hard",
    ([FromRoute] Guid id) =>
    {
        try
        {
            var locationId = LocationId.From(id);

            bool removed = Storage.HardRemove(locationId);

            if (!removed)
                return Results.NotFound($"Локация с ID {id} не найдена.");

            return Results.NoContent();
        }
        catch (Exception)
        {
            return Results.Problem("Ошибка при полном удалении локации.");
        }
    }
);

app.MapDelete(
    "api/positions/{id}",
    ([FromRoute] Guid id) =>
    {
        try
        {
            var positionId = PositionId.From(id);
            var existingPosition = Storage.GetById(positionId);

            if (existingPosition is null)
                return Results.NotFound($"Должность с ID {id} не найдена или архивирована.");

            Storage.Remove(positionId);

            return Results.NoContent();
        }
        catch (Exception)
        {
            return Results.Problem("Ошибка при удалении должности.");
        }
    }
);

app.MapDelete(
    "api/positions/{id}/hard",
    ([FromRoute] Guid id) =>
    {
        try
        {
            var positionId = PositionId.From(id);
            bool removed = Storage.HardRemove(positionId);

            if (!removed)
                return Results.NotFound($"Должность с ID {id} не найдена.");

            return Results.NoContent();
        }
        catch (Exception)
        {
            return Results.Problem("Ошибка при полном удалении должности.");
        }
    }
);
app.Run();

public sealed record CreateLocationRequest(string Address, string Name, string TimeZone);

public sealed record CreatePositionRequest(string Name, string Description, bool IsActive);

public sealed record UpdateLocationRequest(
    string? Address = null,
    string? Name = null,
    string? TimeZone = null
);

public sealed record UpdatePositionRequest(
    string? Name = null,
    string? Description = null,
    bool? IsActive = null
);
