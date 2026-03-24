// DirectoryService.Domain.DepartmentContext.ValueObjects/DepVerification.cs

namespace DirectoryService.Domain.DepartmentContext.ValueObjects;

public class DepVerification
{
	private readonly List<Department> _existingDepartments;

	public DepVerification(List<Department> existingDepartments)
	{
		_existingDepartments = existingDepartments ?? new List<Department>();
	}

	/// <summary>
	/// Проверяет уникальность подразделения по имени
	/// </summary>
	/// <param name="department">Подразделение для проверки</param>
	/// <returns>True если уникально, False если уже существует</returns>
	public bool CheckUniqueness(Department department)
	{
		if (department == null)
			return false;

		// Проверяем есть ли подразделение с таким же именем
		return !_existingDepartments.Any(d => d.Name.Value == department.Name.Value && d.Id != department.Id);
	}

	/// <summary>
	/// Проверяет уникальность по идентификатору
	/// </summary>
	public bool CheckIdentifierUniqueness(DepartmentIdentifier identifier)
	{
		return !_existingDepartments.Any(d => d.Identifier.Value == identifier.Value);
	}
}
