// Domain/DepartmentContext/ValueObjects/DepartmentLifeTime.cs
namespace Domain.DepartmentContext.ValueObjects;

public record DepartmentLifeTime(DateTime CreatedAt, DateTime UpdatedAt)
{
    public static DepartmentLifeTime CreateNew()
    {
        var now = DateTime.UtcNow;
        return new DepartmentLifeTime(now, now);
    }

    public static DepartmentLifeTime Create(DateTime createdAt, DateTime updatedAt)
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

        return new DepartmentLifeTime(createdAt, updatedAt);
    }

    public DepartmentLifeTime Update() => new DepartmentLifeTime(CreatedAt, DateTime.UtcNow);
}
