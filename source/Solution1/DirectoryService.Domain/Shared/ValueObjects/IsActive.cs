// Domain/Shared/ValueObjects/IsActive.cs
namespace Domain.Shared.ValueObjects;

public record IsActive(bool Value)
{
    public static IsActive Create(bool value) => new(value);

    public override string ToString() => Value ? "Active" : "Inactive";
}
