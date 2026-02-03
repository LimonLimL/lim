// Domain/PositionContext/ValueObjects/PositionId.cs
namespace DirectoryService.Domain.PositionContext.ValueObjects;

public record PositionId
{
	public Guid Value { get; }

	private PositionId(Guid value)
	{
		if (value == Guid.Empty)
		{
			throw new ArgumentException("Location ID cannot be empty.", nameof(value));
		}

		Value = value;
	}

	public static PositionId Create()
	{
		return new(Guid.NewGuid());
	}

	public static PositionId From(Guid value)
	{
		if (value == Guid.Empty)
		{
			throw new ArgumentException("Position ID cannot be empty", nameof(value));
		}

		return new PositionId(value);
	}

	public override string ToString()
	{
		return Value.ToString();
	}
}
