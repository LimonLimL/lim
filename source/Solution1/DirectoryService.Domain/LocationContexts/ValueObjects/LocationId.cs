namespace DirectoryService.Domain.LocationsContext.ValueObjects;

public sealed record LocationId
{
	public Guid Value { get; }

	private LocationId(Guid value)
	{
		if (value == Guid.Empty)
		{
			throw new ArgumentException("ID локации не может быть пустым.", nameof(value));
		}

		Value = value;
	}

	public static LocationId Create(Guid id)
	{
		return new(id);
	}

	public static LocationId New()
	{
		return new(Guid.NewGuid());
	}

	public static implicit operator Guid(LocationId locationId)
	{
		return locationId.Value;
	}
}
