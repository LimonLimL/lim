// Domain/DepartmentContext/ValueObjects/DepartmentName.cs
namespace DirectoryService.Domain.DepartmentContext.ValueObjects;

/// <summary>
/// Представляет название отдела как значение (value object).
/// Обеспечивает валидацию: не может быть пустым, null или превышать максимальную длину.
/// </summary>
public record DepartmentName
{
	private DepartmentName(string value)
	{
		Value = value;
	}

	/// <summary>
	/// Значение названия отдела (строка, прошедшая валидацию).
	/// </summary>
	public string Value { get; }
	private const int MaxLength = 100;

	/// <summary>
	/// Создаёт экземпляр <see cref="DepartmentName"/> из строки.
	/// </summary>
	/// <param name="name">Строковое значение названия отдела.</param>
	/// <returns>Новый экземпляр <see cref="DepartmentName"/>.</returns>
	/// <exception cref="ArgumentException">
	/// Выбрасывается, если <paramref name="name"/> является null, пустой строкой, состоит только из пробелов
	/// или превышает <see cref="MaxLength"/> символов.
	/// </exception>
	public static DepartmentName Create(string name)
	{
		if (string.IsNullOrWhiteSpace(name))
		{
			throw new ArgumentException("Department name cannot be empty", nameof(name));
		}

		if (name.Length > MaxLength)
		{
			throw new ArgumentException($"Department name cannot exceed {MaxLength} characters", nameof(name));
		}

		return new DepartmentName(name.Trim());
	}
}
