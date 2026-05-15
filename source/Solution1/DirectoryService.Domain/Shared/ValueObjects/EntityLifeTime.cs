namespace DirectoryService.Domain.Shared.ValueObjects;

public sealed record EntityLifeTime
{
	public DateTime CreatedAt { get; private set; }
	public DateTime? UpdatedAt { get; private set; }
	public DateTime? DeletedAt { get; private set; }
	public bool IsActivate { get; private set; }

	private EntityLifeTime()
	{
		CreatedAt = DateTime.MinValue;
		UpdatedAt = null;
		DeletedAt = null;
		IsActivate = false;
	}

	private EntityLifeTime(DateTime createdAt, DateTime? updatedAt, DateTime? deletedAt, bool isActivate)
	{
		CreatedAt = createdAt;
		UpdatedAt = updatedAt;
		DeletedAt = deletedAt;
		IsActivate = isActivate;
	}

	public static EntityLifeTime Create(DateTime createdAt, DateTime? updatedAt, DateTime? deletedAt, bool isActivate)
	{
		return new EntityLifeTime(createdAt, updatedAt, deletedAt, isActivate);
	}

	public static EntityLifeTime CreateNew()
	{
		var now = DateTime.UtcNow;
		return new EntityLifeTime(now, null, null, true);
	}

	public EntityLifeTime Update()
	{
		return new EntityLifeTime(CreatedAt, DateTime.UtcNow, DeletedAt, IsActivate);
	}

	public EntityLifeTime Deactivate()
	{
		return new EntityLifeTime(CreatedAt, DateTime.UtcNow, DateTime.UtcNow, false);
	}

	public EntityLifeTime Activate()
	{
		return new EntityLifeTime(CreatedAt, DateTime.UtcNow, null, true);
	}
}
