// Domain/Shared/ValueObjects/IsActive.cs
namespace DirectoryService.Domain.Shared.ValueObjects;

public record IsActive(bool Value)
{
	public static IsActive Create(bool value)
	{
		return new(value);
	}

	public override string ToString()
	{
		return Value ? "Active" : "Inactive";
	}
}
