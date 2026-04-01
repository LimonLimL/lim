using DirectoryService.Domain.DepartmentContexts;

namespace DirectoryService.Domain.DepartmentContext.ValueObjects;

public class DepVerification
{
	private readonly List<Department> _existingDepartments;

	public DepVerification(List<Department> existingDepartments)
	{
		_existingDepartments = existingDepartments ?? new List<Department>();
	}

	public bool CheckUniqueness(Department department)
	{
		if (department == null)
			return false;
		return !_existingDepartments.Any(d => d.Name.Value == department.Name.Value && d.Id != department.Id);
	}

	public bool CheckIdentifierUniqueness(DepartmentIdentifier identifier)
	{
		return !_existingDepartments.Any(d => d.Identifier.Value == identifier.Value);
	}
}
