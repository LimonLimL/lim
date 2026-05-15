namespace DirectoryService.Domain.PositionContext.ValueObjects;

public record PositionId
{
	public Guid Value { get; }

	private PositionId()
	{
		Value = Guid.Empty;
	}

	private PositionId(Guid value)
	{
		if (value == Guid.Empty)
		{
			throw new ArgumentException("ID должности не может быть пустым.", nameof(value));
		}
		Value = value;
	}

	public static PositionId Create()
	{
		return new(Guid.NewGuid());
	}

	public static PositionId Create(Guid value)
	{
		if (value == Guid.Empty)
		{
			throw new ArgumentException("ID не может быть пустым.", nameof(value));
		}
		return new(value);
	}

	public static PositionId From(Guid value)
	{
		if (value == Guid.Empty)
		{
			throw new ArgumentException("ID должности не может быть пустым.", nameof(value));
		}
		return new PositionId(value);
	}

	public override string ToString()
	{
		return Value.ToString();
	}
}
