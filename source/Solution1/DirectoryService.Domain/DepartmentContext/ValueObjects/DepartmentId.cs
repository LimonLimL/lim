namespace DirectoryService.Domain.DepartmentContext.ValueObjects;

public record DepartmentId
{
	public Guid Value { get; }

	public static DepartmentId Create()
	{
		return new(Guid.NewGuid());
	}

	private DepartmentId(Guid value)
	{
		if (value == Guid.Empty)
		{
			throw new ArgumentException("ID Локации не может быть пустым.", nameof(value));
		}

		Value = value;
	}

	public static DepartmentId From(Guid value)
	{
		if (value == Guid.Empty)
		{
			throw new ArgumentException("ID Подразделения не может быть пустым.", nameof(value));
		}

		return new DepartmentId(value);
	}

	public override string ToString()
	{
		return Value.ToString();
	}
}
