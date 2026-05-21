using DirectoryService.Domain.LocationContexts.Contracts;
using DirectoryService.Domain.LocationsContext;
using DirectoryService.Domain.LocationsContext.ValueObjects;
using DirectoryService.Domain.Shared.ValueObjects;

namespace Application.LocationsContext.CreateLocation;

public sealed class CreateLocationHandler
{
    private readonly ILocationRepository _repository;

    public CreateLocationHandler(ILocationRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(CreateLocationCommand command, CancellationToken ct = default)
    {
        Validate(command);

        var name = LocationName.Create(command.Name);
        var address = LocationAddress.Create(command.Address);
        var timeZone = IanaTimeZone.Create(command.TimeZone);
        var lifeTime = EntityLifeTime.CreateNew();

        await ValidateUniqueness(name, ct);

        var location = Location.Create(
            Guid.NewGuid(),
            command.Address,
            command.Name,
            command.TimeZone,
            DateTime.UtcNow,
            DateTime.UtcNow
        );

        await _repository.Add(location, ct);

        return location.Id.Value;
    }

    private static void Validate(CreateLocationCommand command)
    {
        if (string.IsNullOrWhiteSpace(command.Name))
        {
            throw new ArgumentException(
                "Название локации не может быть пустым.",
                nameof(command.Name)
            );
        }

        const int maxLength = 255;
        if (command.Name.Length > maxLength)
        {
            throw new ArgumentException(
                $"Название локации не может быть длиннее {maxLength} символов.",
                nameof(command.Name)
            );
        }

        if (string.IsNullOrWhiteSpace(command.Address))
        {
            throw new ArgumentException(
                "Адрес локации не может быть пустым.",
                nameof(command.Address)
            );
        }

        if (string.IsNullOrWhiteSpace(command.TimeZone))
        {
            throw new ArgumentException(
                "Часовой пояс не может быть пустым.",
                nameof(command.TimeZone)
            );
        }
    }

    private async Task ValidateUniqueness(LocationName name, CancellationToken ct)
    {
        var exists = await _repository.Exists(name.Value, ct);
        if (exists)
        {
            throw new InvalidOperationException(
                $"Локация с названием '{name.Value}' уже существует."
            );
        }
    }
}
