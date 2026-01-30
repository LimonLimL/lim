// Domain/DepartmentContext/ValueObjects/DepartmentIdentifier.cs
namespace Domain.DepartmentContext.ValueObjects;

public record DepartmentIdentifier(string Value)
{
    private const int MaxLength = 50;

    public static DepartmentIdentifier Create(string identifier)
    {
        if (string.IsNullOrWhiteSpace(identifier))
            throw new ArgumentException("Identifier cannot be empty", nameof(identifier));

        if (identifier.Length > MaxLength)
            throw new ArgumentException(
                $"Identifier cannot exceed {MaxLength} characters",
                nameof(identifier)
            );

        if (!System.Text.RegularExpressions.Regex.IsMatch(identifier, "^[a-z0-9-]+$"))
            throw new ArgumentException(
                "Identifier must contain only lowercase latin letters, digits and hyphens",
                nameof(identifier)
            );

        return new DepartmentIdentifier(identifier.Trim());
    }

    public static DepartmentIdentifier Root => Create("root");
}
