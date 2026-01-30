// Domain/PositionContext/ValueObjects/PositionName.cs
namespace Domain.PositionContext.ValueObjects;

public record PositionName(string Value)
{
    private const int MaxLength = 100;

    public static PositionName Create(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Position name cannot be empty", nameof(name));

        if (name.Length > MaxLength)
            throw new ArgumentException(
                $"Position name cannot exceed {MaxLength} characters",
                nameof(name)
            );

        return new PositionName(name.Trim());
    }
}
