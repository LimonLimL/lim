public sealed record LocationId
{
    public Guid Value { get; }

    private LocationId(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("Location ID cannot be empty.", nameof(value));

        Value = value;
    }

    public static LocationId Create(Guid id) => new(id);

    public static LocationId New() => new(Guid.NewGuid());

    public static implicit operator Guid(LocationId locationId) => locationId.Value;
}
