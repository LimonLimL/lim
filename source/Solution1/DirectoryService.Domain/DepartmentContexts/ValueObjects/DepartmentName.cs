namespace DirectoryService.Domain.DepartmentContext.ValueObjects;

public record DepartmentName
{
	public DepartmentName(string value)
	{
		Value = value;
	}

	private DepartmentName()
	{
		Value = string.Empty;
	}

	public string Value { get; }
	private const int MaxLength = 100;

	public static DepartmentName Create(string name)
	{
		if (string.IsNullOrWhiteSpace(name))
		{
			throw new ArgumentException("Имя подразделения не может быть пустым.", nameof(name));
		}

		var trimmed = name.Trim();

		if (trimmed.Length > MaxLength)
		{
			throw new ArgumentException($"Имя подразделения не может превышать {MaxLength} символов.", nameof(name));
		}

		return new DepartmentName(trimmed);
	}

	public override string ToString() => Value;
}
