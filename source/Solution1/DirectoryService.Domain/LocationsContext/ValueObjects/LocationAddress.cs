namespace DirectoryService.Domain.LocationsContext.ValueObjects;

/// <summary>
/// Значимый объект, представляющий адрес локации в виде последовательности частей (например: "г. Москва", "ул. Тверская", "д. 1").
/// Обеспечивает неизменяемость, валидацию структуры и нормализацию входных данных.
/// </summary>
/// <remarks>
/// Адрес хранится как список строк (<see cref="AddressParts"/>), разделённых запятыми при преобразовании в строку.
/// При создании через <see cref="Create(string)"/> строка разбивается по запятым, а каждая часть обрезается от пробелов.
/// </remarks>
public class LocationAddress
{
	/// <summary>
	/// Приватное поле для хранения частей адреса (неизменяемый список).
	/// </summary>
	private readonly List<string> _addressParts = [];

	/// <summary>
	/// Возвращает нормализованное строковое представление адреса (части соединены через запятую и пробел).
	/// </summary>
	public string Value { get; }

	/// <summary>
	/// Возвращает только для чтения коллекцию частей адреса.
	/// </summary>
	public IReadOnlyList<string> AddressParts => _addressParts.AsReadOnly();

	private LocationAddress(IEnumerable<string> parts)
	{
		_addressParts = [.. parts];
		Value = string.Join(", ", parts);
	}

	/// <summary>
	/// Создаёт новый экземпляр <see cref="LocationAddress"/> из строки, выполняя следующие проверки:
	/// <list type="bullet">
	///   <item><description>Строка не может быть null, пустой или состоять только из пробельных символов.</description></item>
	///   <item><description>После разбиения по запятым и очистки пробелов должно остаться хотя бы одна непустая часть.</description></item>
	/// </list>
	/// </summary>
	/// <param name="value">Строковое представление адреса (например: "Москва, ул. Ленина, д. 5").</param>
	/// <returns>Новый валидный экземпляр <see cref="LocationAddress"/>.</returns>
	/// <exception cref="ArgumentException">
	/// Выбрасывается, если:
	/// <list type="bullet">
	///   <item><description><paramref name="value"/> — null, пустая строка или состоит только из пробелов.</description></item>
	///   <item><description>После разбиения и нормализации получается пустой список частей (например, ", , ").</description></item>
	/// </list>
	/// </exception>
	public static LocationAddress Create(string value)
	{
		if (string.IsNullOrWhiteSpace(value))
		{
			throw new ArgumentException("Адрес локации не может быть пустым.", nameof(value));
		}

		List<string> parts =
		[
			.. value.Split(',').Select(part => part.Trim()).Where(part => !string.IsNullOrWhiteSpace(part)),
		];

		if (parts.Count == 0)
		{
			throw new ArgumentException("Адрес локации должен содержать хотя бы одну часть.", nameof(value));
		}

		return new LocationAddress(parts);
	}
}
