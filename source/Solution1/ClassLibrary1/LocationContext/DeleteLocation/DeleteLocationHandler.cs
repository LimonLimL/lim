using DirectoryService.Domain.LocationContexts.Contracts;

public sealed class DeleteLocationHandler
{
    private readonly ILocationRepository _repository;

    public DeleteLocationHandler(ILocationRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(DeleteLocationCommand command, CancellationToken ct = default)
    {
        var location = await _repository.GetById(command.Id, ct);

        if (location is null)
        {
            throw new InvalidOperationException("Локация не найдена.");
        }

        await _repository.Delete(location, ct);

        return location.Id.Value;
    }
}
