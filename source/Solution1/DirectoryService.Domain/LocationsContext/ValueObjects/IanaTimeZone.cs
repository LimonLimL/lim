namespace DirectoryService.Domain.LocationsContext.ValueObjects;

/// <summary>
/// Значимый объект, представляющий IANA-идентификатор часового пояса (например, "Europe/Moscow").
/// Обеспечивает неизменяемость и валидацию формата часового пояса при создании.
/// </summary>
/// <remarks>
/// Используется как значение-объект (value object) в доменной модели — сравнивается по значению, а не по ссылке.
/// </remarks>
public sealed record IanaTimeZone
{
	/// <summary>
	/// Возвращает IANA-идентификатор часового пояса (например, "America/New_York").
	/// </summary>
	public string TimeZone { get; }

	private IanaTimeZone(string timeZone)
	{
		TimeZone = timeZone;
	}

	/// <summary>
	/// Создаёт новый экземпляр <see cref="IanaTimeZone"/> после проверки корректности входного значения.
	/// </summary>
	/// <param name="timeZone">Строка с IANA-идентификатором часового пояса.</param>
	/// <returns>Новый экземпляр <see cref="IanaTimeZone"/>.</returns>
	/// <exception cref="ArgumentException">
	/// Выбрасывается, если <paramref name="timeZone"/> является null, пустой строкой или состоит только из пробельных символов.
	/// </exception>
	public static IanaTimeZone Create(string timeZone)
	{
		if (string.IsNullOrWhiteSpace(timeZone))
		{
			throw new ArgumentException("Часовой пояс не может быть пустым.", nameof(timeZone));
		}

		return new IanaTimeZone(timeZone);
	}
}
