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
			throw new ArgumentException("ID локации не может быть пустым.", nameof(value));
		}

		Value = value;
	}

	public static DepartmentId From(Guid value)
	{
		if (value == Guid.Empty)
		{
			throw new ArgumentException("ID подразделения не может быть пустым.", nameof(value));
		}

		return new DepartmentId(value);
	}

	public override string ToString()
	{
		return Value.ToString();
	}

	public static DepartmentId From(Guid? value)
	{
		if (value is null || value == Guid.Empty)
		{
			throw new ArgumentException("ID не может быть null или пустым.", nameof(value));
		}

		return new DepartmentId(value.Value);
	}

	private DepartmentId()
	{
		Value = Guid.Empty;
	}
}
