namespace DirectoryService.Domain.DepartmentContext.ValueObjects;

public record DepartmentName
{
	private DepartmentName(string value)
	{
		Value = value;
	}

	public string Value { get; }
	private const int MaxLength = 100;

	public static DepartmentName Create(string name)
	{
		if (string.IsNullOrWhiteSpace(name))
		{
			throw new ArgumentException("Department name cannot be empty", nameof(name));
		}

		if (name.Length > MaxLength)
		{
			throw new ArgumentException($"Department name cannot exceed {MaxLength} characters", nameof(name));
		}

		return new DepartmentName(name.Trim());
	}
}
