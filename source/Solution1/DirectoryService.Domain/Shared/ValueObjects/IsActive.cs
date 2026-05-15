namespace DirectoryService.Domain.Shared.ValueObjects;

public record IsActive(bool Value)
{
	private IsActive()
		: this(false) { }

	public static IsActive Create(bool value)
	{
		return new(value);
	}

	public override string ToString()
	{
		return Value ? "Active" : "Inactive";
	}
}
