namespace DirectoryService.Domain.LocationsContext.ValueObjects;

/// <summary>
/// Значимый объект, представляющий уникальный идентификатор локации на основе <see cref="Guid"/>.
/// Обеспечивает неизменяемость, валидацию и удобное создание через статические методы.
/// </summary>
/// <remarks>
/// Используется как value object в доменной модели — сравнение происходит по значению GUID,
/// а не по ссылке. Конструкторы защищены от создания пустых идентификаторов.
/// </remarks>
public sealed record LocationId
{
	/// <summary>
	/// Приватный конструктор, запрещающий создание экземпляра с пустым GUID.
	/// </summary>
	/// <param name="value">Значение GUID. Должно быть не равным <see cref="Guid.Empty"/>.</param>
	/// <exception cref="ArgumentException">Выбрасывается, если <paramref name="value"/> равен <see cref="Guid.Empty"/>.</exception>
	public Guid Value { get; }

	private LocationId(Guid value)
	{
		if (value == Guid.Empty)
		{
			throw new ArgumentException("Location ID cannot be empty.", nameof(value));
		}

		Value = value;
	}

	/// <summary>
	/// Создаёт новый экземпляр <see cref="LocationId"/> из указанного GUID.
	/// </summary>
	/// <param name="id">GUID, который будет использоваться как идентификатор локации.</param>
	/// <returns>Новый экземпляр <see cref="LocationId"/>.</returns>
	/// <exception cref="ArgumentException">
	/// Выбрасывается, если <paramref name="id"/> равен <see cref="Guid.Empty"/>.
	/// </exception>
	public static LocationId Create(Guid id)
	{
		return new(id);
	}

	/// <summary>
	/// Создаёт новый экземпляр <see cref="LocationId"/> с новым случайным GUID (UUIDv4).
	/// </summary>
	/// <returns>Новый экземпляр <see cref="LocationId"/> с уникальным значением.</returns>
	public static LocationId New()
	{
		return new(Guid.NewGuid());
	}

	public static implicit operator Guid(LocationId locationId)
	{
		return locationId.Value;
	}
}
