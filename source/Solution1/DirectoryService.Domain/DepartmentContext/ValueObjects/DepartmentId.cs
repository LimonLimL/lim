// Domain/DepartmentContext/ValueObjects/DepartmentId.cs
namespace DirectoryService.Domain.DepartmentContext.ValueObjects;

/// <summary>
/// Идентификатор отдела — значение (value object), основанное на GUID.
/// Обеспечивает типобезопасность и валидацию: запрещает использование пустого GUID.
/// </summary>
public record DepartmentId
{
	public Guid Value { get; }

	/// <summary>
	/// Создаёт новый уникальный идентификатор отдела с использованием сгенерированного GUID.
	/// </summary>
	/// <returns>Новый экземпляр <see cref="DepartmentId"/> с новым GUID.</returns>
	public static DepartmentId Create()
	{
		return new(Guid.NewGuid());
	}

	private DepartmentId(Guid value)
	{
		if (value == Guid.Empty)
		{
			throw new ArgumentException("Location ID cannot be empty.", nameof(value));
		}

		Value = value;
	}

	/// <summary>
	/// Создаёт идентификатор отдела из заданного GUID.
	/// </summary>
	/// <param name="value">GUID, который будет использоваться как идентификатор отдела.</param>
	/// <returns>Новый экземпляр <see cref="DepartmentId"/> с указанным GUID.</returns>
	/// <exception cref="ArgumentException">
	/// Выбрасывается, если переданный <paramref name="value"/> равен <see cref="Guid.Empty"/>.
	/// </exception>
	public static DepartmentId From(Guid value)
	{
		if (value == Guid.Empty)
		{
			throw new ArgumentException("Department ID cannot be empty", nameof(value));
		}

		return new DepartmentId(value);
	}

	/// <summary>
	/// Возвращает строковое представление GUID (в стандартном формате, например: "a1b2c3d4-...").
	/// Переопределено для удобства отладки и логирования.
	/// </summary>
	/// <returns>Строковое представление значения GUID.</returns>
	public override string ToString()
	{
		return Value.ToString();
	}
}
