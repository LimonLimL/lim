using DirectoryService.Domain.PositionContext;
using DirectoryService.Domain.PositionContext.ValueObjects;

namespace DirectoryService.Domain.PositionContexts.Contracts;

public interface IPositionRepository
{
	Task<bool> Exists(string name, CancellationToken ct = default);
	Task Add(Position position, CancellationToken ct = default);
	Task<Position?> GetById(Guid id, CancellationToken ct = default);
	Task<Position?> GetByName(PositionName name, CancellationToken ct = default);
	Task Update(Position position, CancellationToken ct = default);
	Task<IEnumerable<Position>> GetAll(CancellationToken ct = default);
	Task<IEnumerable<Position>> GetManyByIds(IEnumerable<Guid> ids, CancellationToken ct = default);
	Task DeleteMany(IEnumerable<Position> positions, CancellationToken ct = default);
}
