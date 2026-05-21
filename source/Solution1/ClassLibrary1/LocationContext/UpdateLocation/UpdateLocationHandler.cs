using DirectoryService.Domain.LocationContexts.Contracts;
using DirectoryService.Domain.LocationsContext.ValueObjects;

public sealed class UpdateLocationHandler
{
    private readonly ILocationRepository _repository;

    public UpdateLocationHandler(ILocationRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(UpdateLocationCommand command, CancellationToken ct = default)
    {
        var location = await _repository.GetById(command.Id, ct);
        if (location is null)
        {
            throw new InvalidOperationException($"Локация с ID {command.Id} не найдена.");
        }

        LocationName? newName = command.NewName is not null
            ? LocationName.Create(command.NewName)
            : null;

        LocationAddress? newAddress = command.NewAddress is not null
            ? LocationAddress.Create(command.NewAddress)
            : null;

        location.Update(newName, newAddress);

        await _repository.Update(location, ct);

        return location.Id.Value;
    }
}
