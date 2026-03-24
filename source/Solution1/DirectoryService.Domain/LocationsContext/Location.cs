using System.Xml.Linq;
using DirectoryService.Domain.LocationsContext.ValueObjects;
using DirectoryService.Domain.Shared.ValueObjects;

namespace DirectoryService.Domain.LocationsContext;

public class Location
{
	public void Rename(LocationName other)
	{
		if (LifeTime.IsActivate == false)
			throw new InvalidOperationException("Сущнность архивированна");
		Name = other;
		LifeTime = LifeTime.Update();
	}

	public void Retime(IanaTimeZone other)
	{
		if (LifeTime.IsActivate == false)
			throw new InvalidOperationException("Сущнность архивированна");
		TimeZone = other;
		LifeTime = LifeTime.Update();
	}

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
	public LocationName Name { get; set; }
	public LocationAddress Address { get; }
	public EntityLifeTime LifeTime { get; set; }
	public IanaTimeZone TimeZone { get; set; }

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
		{
			throw new ArgumentNullException(nameof(id), "Идентификатор не может быть пустым.");
		}

		if (string.IsNullOrWhiteSpace(name))
		{
			throw new ArgumentNullException(nameof(name), "Название локации не может быть пустым.");
		}

		if (string.IsNullOrWhiteSpace(address))
		{
			throw new ArgumentNullException(nameof(address), "Адрес локации не может быть пустым.");
		}

		if (createdAt == DateTime.MinValue || createdAt == DateTime.MaxValue)
		{
			throw new ArgumentNullException(nameof(createdAt), "Некорректное значение даты создания.");
		}

		if (updatedAt == DateTime.MinValue || updatedAt == DateTime.MaxValue)
		{
			throw new ArgumentNullException(nameof(updatedAt), "Некорректное значение даты обновления.");
		}

		LocationId locationId = LocationId.Create(id);
		LocationName locationName = LocationName.Create(name);
		LocationAddress locationAddress = LocationAddress.Create(address);
		IanaTimeZone ianaTimeZone = IanaTimeZone.Create(timeZone);
		EntityLifeTime entityLifeTime = EntityLifeTime.Create(createdAt, updatedAt, true);
		return new Location(locationId, locationAddress, locationName, ianaTimeZone, entityLifeTime);
	}
}
