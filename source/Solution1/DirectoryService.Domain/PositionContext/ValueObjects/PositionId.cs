// Domain/PositionContext/ValueObjects/PositionId.cs
namespace Domain.PositionContext.ValueObjects;

public record PositionId(Guid Value)
{
    public static PositionId Create() => new(Guid.NewGuid());

    public static PositionId From(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("Position ID cannot be empty", nameof(value));

        return new PositionId(value);
    }

    public override string ToString() => Value.ToString();
}
