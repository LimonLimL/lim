using DirectoryService.Domain.PositionContext;
using DirectoryService.Domain.PositionContext.ValueObjects;
using DirectoryService.Domain.PositionContexts.Contracts;
using DirectoryService.Domain.Shared.ValueObjects;

public sealed class CreatePositionHandler
{
    private readonly IPositionRepository _repository;

    public CreatePositionHandler(IPositionRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(CreatePositionCommand command, CancellationToken ct = default)
    {
        Validate(command);

        var name = PositionName.Create(command.Name);
        var description = PositionDescription.Create(command.Description ?? string.Empty);
        var isActive = IsActive.Create(true);
        var lifeTime = EntityLifeTime.CreateNew();

        await ValidateUniqueness(name, ct);
        var position = Position.Create(
            name,
            description,
            isActive,
            lifeTime,
            new PosVerification(new List<Position>())
        );

        await _repository.Add(position, ct);

        return position.Id.Value;
    }

    private static void Validate(CreatePositionCommand command)
    {
        if (string.IsNullOrWhiteSpace(command.Name))
        {
            throw new ArgumentException(
                "Название должности не может быть пустым.",
                nameof(command.Name)
            );
        }

        const int maxLength = 255;
        if (command.Name.Length > maxLength)
        {
            throw new ArgumentException(
                $"Название должности не может быть длиннее {maxLength} символов.",
                nameof(command.Name)
            );
        }
    }

    private async Task ValidateUniqueness(PositionName name, CancellationToken ct)
    {
        var exists = await _repository.Exists(name.Value, ct);
        if (exists)
        {
            throw new InvalidOperationException(
                $"Должность с названием '{name.Value}' уже существует."
            );
        }
    }
}
