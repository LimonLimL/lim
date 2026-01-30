// Domain/DepartmentContext/ValueObjects/DepartmentPath.cs
namespace Domain.DepartmentContext.ValueObjects;

public record DepartmentPath(string Value)
{
    private const int MaxLength = 255;

    public static DepartmentPath CreateRoot() => new(string.Empty);

    public static DepartmentPath Create(DepartmentPath parentPath, DepartmentIdentifier identifier)
    {
        var pathValue = string.IsNullOrEmpty(parentPath.Value)
            ? identifier.Value
            : $"{parentPath.Value}.{identifier.Value}";

        if (pathValue.Length > MaxLength)
            throw new ArgumentException(
                $"Path cannot exceed {MaxLength} characters",
                nameof(pathValue)
            );

        return new DepartmentPath(pathValue);
    }

    public bool IsRoot => string.IsNullOrEmpty(Value);
}
