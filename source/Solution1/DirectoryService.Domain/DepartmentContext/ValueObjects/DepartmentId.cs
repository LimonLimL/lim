// Domain/DepartmentContext/ValueObjects/DepartmentId.cs
namespace Domain.DepartmentContext.ValueObjects;

public record DepartmentId(Guid Value)
{
    public static DepartmentId Create() => new(Guid.NewGuid());

    public static DepartmentId From(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("Department ID cannot be empty", nameof(value));

        return new DepartmentId(value);
    }

    public override string ToString() => Value.ToString();
}
