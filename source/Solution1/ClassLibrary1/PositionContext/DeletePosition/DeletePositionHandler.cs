using DirectoryService.Domain.PositionContexts.Contracts;

public sealed class DeletePositionHandler
{
    private readonly IPositionRepository _repository;

    public DeletePositionHandler(IPositionRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Guid>> Handle(
        DeletePositionCommand command,
        CancellationToken ct = default
    )
    {
        var positions = await _repository.GetManyByIds(command.Ids, ct);

        var idsList = command.Ids.ToList();
        var foundIds = positions.Select(p => p.Id.Value).ToList();

        var notFound = idsList.Except(foundIds).ToList();
        if (notFound.Any())
        {
            throw new InvalidOperationException("Должности не найдены.");
        }

        await _repository.DeleteMany(positions, ct);

        return foundIds;
    }
}
