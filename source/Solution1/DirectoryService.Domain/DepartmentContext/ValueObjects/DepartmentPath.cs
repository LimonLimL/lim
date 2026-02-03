// Domain/DepartmentContext/ValueObjects/DepartmentPath.cs
namespace DirectoryService.Domain.DepartmentContext.ValueObjects;

/// <summary>
/// Путь к отделу в иерархии (например, "it/hr/finance").
/// Представляет собой строку, состоящую из идентификаторов отделов, разделённых слешами (/).
/// Обеспечивает валидацию: не может быть пустым, превышать 255 символов, содержать недопустимые символы.
/// </summary>
public record DepartmentPath
{
	public string Value { get; }

	private DepartmentPath(string value)
	{
		Value = value;
	}

	private const int MaxLength = 255;

	/// <summary>
	/// Создаёт пустой путь — используется корневого отдела.
	/// Значение — пустая строка.
	/// </summary>
	/// <returns>Экземпляр <see cref="DepartmentPath"/> с пустым значением.</returns>
	public static DepartmentPath CreateRoot()
	{
		return new(string.Empty);
	}

	/// <summary>
	/// Создаёт путь на основе родительского пути и идентификатора текущего отдела.
	/// Формат: <c>{parentPath}/{identifier}</c>.
	/// Выполняет валидацию длины и допустимых символов.
	/// </summary>
	/// <param name="parentPath">Родительский путь (может быть пустым для корня).</param>
	/// <param name="identifier">Идентификатор текущего отдела (должен быть валидным <see cref="DepartmentIdentifier"/>).</param>
	/// <returns>Новый экземпляр <see cref="DepartmentPath"/>.</returns>
	/// <exception cref="ArgumentException">
	/// Выбрасывается, если:
	/// <list type="bullet">
	///   <item><description>результирующий путь превышает <see cref="MaxLength"/> символов;</description></item>
	///   <item><description>в пути встречаются недопустимые символы (разрешены только: строчные латинские буквы, цифры, дефисы, слеши и точки).</description></item>
	/// </list>
	/// </exception>
	public static DepartmentPath Create(DepartmentPath parentPath, DepartmentIdentifier identifier)
	{
		string newValue = string.IsNullOrEmpty(parentPath.Value)
			? identifier.Value
			: $"{parentPath.Value}.{identifier.Value}";

		// Валидация длины
		if (newValue.Length > MaxLength)
		{
			throw new ArgumentException($"Путь не может превышать {MaxLength} символов", nameof(parentPath)); // ← Исправлено: передаём имя переменной, а не параметра
		}

		// Валидация символов: разрешены a-z, 0-9, -, ., /
		if (!System.Text.RegularExpressions.Regex.IsMatch(newValue, @"^[a-z0-9\-\.\/]+$"))
		{
			throw new ArgumentException(
				"Путь должен содержать только строчные латинские буквы, цифры, дефисы, точки и слеши",
				nameof(parentPath)
			);
		}

		return new DepartmentPath(newValue);
	}

	/// <summary>
	/// Проверяет, является ли текущий путь корневым (пустой строкой).
	/// </summary>
	/// <returns><see langword="true"/>, если путь пустой; иначе <see langword="false"/>.</returns>
	public bool IsRoot => string.IsNullOrEmpty(Value);
}
