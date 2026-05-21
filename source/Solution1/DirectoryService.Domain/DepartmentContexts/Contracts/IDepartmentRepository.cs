using DirectoryService.Domain.DepartmentContext.ValueObjects;
using DirectoryService.Domain.DepartmentContexts;

public interface IDepartmentRepository
{
	Task<Department?> GetById(Guid id, CancellationToken ct = default);
	Task<IEnumerable<Department>> GetAll(CancellationToken ct = default);
	Task<bool> Exists(string name, CancellationToken ct = default);
	Task<Department?> GetByName(DepartmentName name, CancellationToken ct = default);
	Task Add(Department department, CancellationToken ct = default);
	Task Update(Department department, CancellationToken ct = default);
	Task Delete(Department department, CancellationToken ct = default);
}
