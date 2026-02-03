// Domain/PositionContext/ValueObjects/PositionDescription.cs
namespace DirectoryService.Domain.PositionContext.ValueObjects;

public record PositionDescription
{
	public string Value { get; }

	private PositionDescription(string value)
	{
		Value = value;
	}

	private const int MaxLength = 500;

	public static PositionDescription Create(string description)
	{
		if (description == null)
		{
			return new PositionDescription(string.Empty);
		}

		if (description.Length > MaxLength)
		{
			throw new ArgumentException($"Description cannot exceed {MaxLength} characters", nameof(description));
		}

		return new PositionDescription(description.Trim());
	}

	public bool IsEmpty => string.IsNullOrEmpty(Value);
}
