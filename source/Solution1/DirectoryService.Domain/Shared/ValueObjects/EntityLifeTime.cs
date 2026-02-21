namespace DirectoryService.Domain.Shared.ValueObjects;

/// <summary>
/// Значимый объект, представляющий временные метки жизненного цикла сущности (например, даты создания, обновления и флаг активности).
/// Используется для обеспечения неизменяемости и валидации временных данных при создании доменных объектов.
/// </summary>
public sealed class EntityLifeTime
{
	/// <summary>
	/// Дата и время создания сущности.
	/// </summary>
	public DateTime CreatedAt { get; }

	/// <summary>
	/// Дата и время последнего обновления сущности.
	/// </summary>
	public DateTime UpdatedAt { get; }

	/// <summary>
	/// Флаг, указывающий, активна ли сущность (например, не удалена логически).
	/// </summary>
	public bool IsActivate { get; }

	private EntityLifeTime(DateTime createdAt, DateTime updatedAt, bool isActivate)
	{
		CreatedAt = createdAt;
		UpdatedAt = updatedAt;
		IsActivate = isActivate;
	}

	public EntityLifeTime Update()
	{
		DateTime UTC = DateTime.UtcNow;
		return new EntityLifeTime(CreatedAt, UTC, IsActivate);
	}

	/// <summary>
	/// Создаёт новый экземпляр <see cref="EntityLifeTime"/>, выполняя строгую валидацию входных значений:
	/// <list type="bullet">
	///   <item><description>Проверка на недопустимые значения дат (<see cref="DateTime.MinValue"/> или <see cref="DateTime.MaxValue"/>).</description></item>
	///   <item><description>Проверка, что дата обновления не меньше даты создания.</description></item>
	/// </list>
	/// </summary>
	/// <param name="createdAt">Дата создания сущности.</param>
	/// <param name="updatedAt">Дата последнего обновления сущности.</param>
	/// <param name="isActivate">Флаг активности сущности.</param>
	/// <returns>Новый валидный экземпляр <see cref="EntityLifeTime"/>.</returns>
	/// <exception cref="ArgumentException">
	/// Выбрасывается, если:
	/// <list type="bullet">
	///   <item><description><paramref name="createdAt"/> равна <see cref="DateTime.MinValue"/> или <see cref="DateTime.MaxValue"/>.</description></item>
	///   <item><description><paramref name="updatedAt"/> равна <see cref="DateTime.MinValue"/> или <see cref="DateTime.MaxValue"/>.</description></item>
	///   <item><description><paramref name="updatedAt"/> меньше <paramref name="createdAt"/>.</description></item>
	/// </list>
	/// </exception>
	public static EntityLifeTime Create(DateTime createdAt, DateTime updatedAt, bool isActivate)
	{
		if (createdAt == DateTime.MinValue || createdAt == DateTime.MaxValue)
		{
			throw new ArgumentException("Неккорректное значение даты создания.", nameof(createdAt));
		}

		if (updatedAt == DateTime.MinValue || updatedAt == DateTime.MaxValue)
		{
			throw new ArgumentException("Некорректное значение даты обновления.", nameof(updatedAt));
		}

		if (updatedAt < createdAt)
		{
			throw new ArgumentException("Дата обновления не может быть меньше даты создания.", nameof(updatedAt));
		}

		return new EntityLifeTime(createdAt, updatedAt, isActivate);
	}
}
