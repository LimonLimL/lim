using DirectoryService.Domain.LocationsContext;
using DirectoryService.Domain.PositionContext;
using DirectoryService.Domain.PositionContext.ValueObjects;

namespace DirectoryService.Domain.LocationContexts.Contracts;

public interface ILocationRepository
{
	Task<bool> Exists(string name, CancellationToken ct = default);
	Task Add(Location location, CancellationToken ct = default);
	Task<Location?> GetById(Guid id, CancellationToken ct = default);
	Task Update(Location position, CancellationToken ct = default);
	Task<IEnumerable<Location>> GetAll(CancellationToken ct = default);
	Task Delete(Location location, CancellationToken ct = default);
}
