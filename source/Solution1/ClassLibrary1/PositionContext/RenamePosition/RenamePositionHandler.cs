using DirectoryService.Domain.PositionContext.ValueObjects;
using DirectoryService.Domain.PositionContexts.Contracts;

public sealed class RenamePositionHandler
{
    private readonly IPositionRepository _repository;

    public RenamePositionHandler(IPositionRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(RenamePositionCommand command, CancellationToken ct = default)
    {
        var position = await _repository.GetById(command.Id, ct);
        if (position is null)
        {
            throw new InvalidOperationException($"Должность с ID {command.Id} не найдена.");
        }
        var newName = PositionName.Create(command.NewName);
        var duplicate = await _repository.GetByName(newName, ct);
        if (duplicate is not null && duplicate.Id != position.Id)
        {
            throw new InvalidOperationException(
                $"Должность с названием '{command.NewName}' уже существует."
            );
        }
        position.Rename(newName);
        await _repository.Update(position, ct);

        return position.Id.Value;
    }
}
