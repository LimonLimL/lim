namespace DirectoryService.Domain.PositionContext.ValueObjects;

public sealed record Rank
{
	public int Value { get; }

	private Rank()
	{
		Value = 0;
	}

	private Rank(int value)
	{
		if (value < 1)
			throw new ArgumentException("Ранг должен быть >= 1.", nameof(value));
		Value = value;
	}

	public static Rank Create(int value) => new(value);

	public static Rank Initial() => new(1);
}
