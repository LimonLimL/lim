namespace DirectoryService.Domain.Shared.ValueObjects;

public sealed class EntityLifeTime
{
	public DateTime CreatedAt { get; }
	public DateTime UpdatedAt { get; }
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

	internal static EntityLifeTime CreateNew()
	{
		DateTime now = DateTime.UtcNow;
		return new EntityLifeTime(now, now, true);
	}
}
