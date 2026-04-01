namespace DirectoryService.Domain.PositionContext.ValueObjects;

public record PositionName
{
	private PositionName(string value)
	{
		Value = value;
	}

	public string Value { get; }

	private const int MaxLength = 100;

	public static PositionName Create(string name)
	{
		if (string.IsNullOrWhiteSpace(name))
		{
			throw new ArgumentException("Имя должности не может быть пустым.", nameof(name));
		}

		if (name.Length > MaxLength)
		{
			throw new ArgumentException($"Имя должности не может превышать {MaxLength} символов.", nameof(name));
		}

		return new PositionName(name.Trim());
	}
}
