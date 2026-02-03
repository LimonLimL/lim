// Domain/DepartmentContext/ValueObjects/DepartmentIdentifier.cs
namespace DirectoryService.Domain.DepartmentContext.ValueObjects;

/// <summary>
/// Уникальный текстовый идентификатор отдела (например, "it", "hr-2025", "finance-main").
/// Является value object: неизменяемый, с валидацией при создании.
/// Допустимые символы: строчные латинские буквы, цифры и дефисы (-).
/// Максимальная длина — 50 символов.
/// </summary>
public record DepartmentIdentifier
{
	public string Value { get; }

	private DepartmentIdentifier(string value)
	{
		Value = value;
	}

	/// <summary>
	/// Максимально допустимая длина идентификатора (50 символов).
	/// </summary>
	private const int MaxLength = 50;

	/// <summary>
	/// Создаёт экземпляр <see cref="DepartmentIdentifier"/> из строки.
	/// Выполняет валидацию: проверяет на пустоту, длину и соответствие шаблону.
	/// </summary>
	/// <param name="identifier">Строковое значение идентификатора.</param>
	/// <returns>Новый экземпляр <see cref="DepartmentIdentifier"/>.</returns>
	/// <exception cref="ArgumentException">
	/// Выбрасывается, если:
	/// <list type="bullet">
	///   <item><description>идентификатор пустой или состоит только из пробелов;</description></item>
	///   <item><description>длина превышает <see cref="MaxLength"/>;</description></item>
	///   <item><description>содержит недопустимые символы (не строчные латинские буквы, цифры или дефисы).</description></item>
	/// </list>
	/// </exception>
	public static DepartmentIdentifier Create(string identifier)
	{
		if (string.IsNullOrWhiteSpace(identifier))
		{
			throw new ArgumentException("Identifier cannot be empty", nameof(identifier));
		}

		if (identifier.Length > MaxLength)
		{
			throw new ArgumentException($"Identifier cannot exceed {MaxLength} characters", nameof(identifier));
		}

		if (!System.Text.RegularExpressions.Regex.IsMatch(identifier, "^[a-z0-9-]+$"))
		{
			throw new ArgumentException(
				"Identifier must contain only lowercase latin letters, digits and hyphens",
				nameof(identifier)
			);
		}

		return new DepartmentIdentifier(identifier.Trim());
	}

	/// <summary>
	/// Возвращает корневой идентификатор — строку "root".
	/// Используется для обозначения корневого отдела в иерархии.
	/// </summary>
	public static DepartmentIdentifier Root => Create("root");
}
