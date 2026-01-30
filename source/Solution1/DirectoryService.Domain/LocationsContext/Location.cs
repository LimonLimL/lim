public class Location
{
    public Location(
        LocationId id,
        LocationAddress address,
        LocationName name,
        IanaTimeZone timeZone,
        EntityLifeTime lifeTime
    )
    {
        Id = id;
        Address = address;
        Name = name;
        TimeZone = timeZone;
        LifeTime = lifeTime;
    }

    public LocationId Id { get; }
    public LocationName Name { get; }
    public LocationAddress Address { get; }
    public EntityLifeTime LifeTime { get; }
    public IanaTimeZone TimeZone { get; }

    public static Location Create(
        Guid id,
        string address,
        string name,
        string timeZone,
        DateTime createdAt,
        DateTime updatedAt
    )
    {
        if (id == Guid.Empty)
            throw new ArgumentNullException("Идентификатор не может быть пустым.", nameof(id));

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentNullException("Название локации не может быть пустым.", nameof(name));

        if (string.IsNullOrWhiteSpace(address))
            throw new ArgumentNullException("Адрес локации не может быть пустым.", nameof(address));

        if (createdAt == DateTime.MinValue || createdAt == DateTime.MaxValue)
            throw new ArgumentNullException(
                "Некорректное значение даты создания.",
                nameof(createdAt)
            );

        if (updatedAt == DateTime.MinValue || updatedAt == DateTime.MaxValue)
            throw new ArgumentNullException(
                "Некорректное значение даты обновления.",
                nameof(updatedAt)
            );
        var locationId = LocationId.Create(id);
        var locationName = LocationName.Create(name);
        var locationAddress = LocationAddress.Create(address);
        var ianaTimeZone = IanaTimeZone.Create(timeZone);
        var entityLifeTime = EntityLifeTime.Create(createdAt, updatedAt, true);
        return new Location(
            locationId,
            locationAddress,
            locationName,
            ianaTimeZone,
            entityLifeTime
        );
    }
}
