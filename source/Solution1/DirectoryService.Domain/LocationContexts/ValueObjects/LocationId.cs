namespace DirectoryService.Domain.LocationsContext.ValueObjects
{
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

		public static LocationId Create()
		{
			return new(Guid.NewGuid());
		}

		public static LocationId Create(Guid id)
		{
			return new(id);
		}

		public static LocationId From(Guid guid)
		{
			return new(guid);
		}

		public static implicit operator Guid(LocationId locationId)
		{
			return locationId.Value;
		}

		public override string ToString()
		{
			return Value.ToString();
		}

		private LocationId()
		{
			Value = Guid.Empty;
		}
	}
}
