// Domain/PositionContext/Position.cs
using DirectoryService.Domain.LocationsContext.ValueObjects;
using DirectoryService.Domain.PositionContext.ValueObjects;
using DirectoryService.Domain.Shared.ValueObjects;

namespace DirectoryService.Domain.PositionContext;

public class Position
{
	public void Rename(PositionName other)
	{
		if (LifeTime.IsActivate == false)
			throw new InvalidOperationException("Сущнность архивированна");
		Name = other;
		LifeTime = LifeTime.Update();
	}

	public PositionId Id { get; }
	public PositionName Name { get; set; }
	public PositionDescription Description { get; set; }
	public IsActive IsActive { get; set; }
	public EntityLifeTime LifeTime { get; set; }

	// Приватный конструктор
	private Position(
		PositionId id,
		PositionName name,
		PositionDescription description,
		IsActive isActive,
		EntityLifeTime lifeTime
	)
	{
		Id = id;
		Name = name;
		Description = description;
		IsActive = isActive;
		LifeTime = lifeTime;
	}

	// Фабричный метод для создания новой позиции
	public static Position Create(
		PositionName name,
		PositionDescription description,
		IsActive isActive,
		EntityLifeTime lifeTime,
		PosVerification posVerification
	)
	{
		Position Tocheck = new Position(PositionId.Create(), name, description, isActive, lifeTime);
		bool PosVer = posVerification.СheckUniqueness(Tocheck);
		if (PosVer == false)
			throw new InvalidOperationException($"{name} уже существует");
		return Tocheck;
	}

	// Фабричный метод для восстановления из хранилища
	public static Position Restore(
		PositionId id,
		PositionName name,
		PositionDescription description,
		IsActive isActive,
		EntityLifeTime lifeTime
	)
	{
		return new Position(id, name, description, isActive, lifeTime);
	}

	// Метод для обновления позиции
	public void Update(PositionName name, PositionDescription description, IsActive isActive)
	{
		Name = name;
		Description = description;
		IsActive = isActive;
	}
}
