// Domain/PositionContext/ValueObjects/PositionLifeTime.cs
namespace Domain.PositionContext.ValueObjects;

public record PositionLifeTime(DateTime CreatedAt, DateTime UpdatedAt)
{
    public static PositionLifeTime CreateNew()
    {
        var now = DateTime.UtcNow;
        return new PositionLifeTime(now, now);
    }

    public static PositionLifeTime Create(DateTime createdAt, DateTime updatedAt)
    {
        if (createdAt.Kind != DateTimeKind.Utc)
            throw new ArgumentException("CreatedAt must be in UTC", nameof(createdAt));

        if (updatedAt.Kind != DateTimeKind.Utc)
            throw new ArgumentException("UpdatedAt must be in UTC", nameof(updatedAt));

        if (updatedAt < createdAt)
            throw new ArgumentException(
                "UpdatedAt cannot be earlier than CreatedAt",
                nameof(updatedAt)
            );

        return new PositionLifeTime(createdAt, updatedAt);
    }

    public PositionLifeTime Update() => new PositionLifeTime(CreatedAt, DateTime.UtcNow);
}
