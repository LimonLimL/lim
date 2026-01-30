// Domain/DepartmentContext/ValueObjects/DepartmentName.cs
namespace Domain.DepartmentContext.ValueObjects;

public record DepartmentName(string Value)
{
    private const int MaxLength = 100;

    public static DepartmentName Create(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Department name cannot be empty", nameof(name));

        if (name.Length > MaxLength)
            throw new ArgumentException(
                $"Department name cannot exceed {MaxLength} characters",
                nameof(name)
            );

        return new DepartmentName(name.Trim());
    }
}
