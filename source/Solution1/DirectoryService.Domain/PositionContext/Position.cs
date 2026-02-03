// Domain/PositionContext/Position.cs
using DirectoryService.Domain.PositionContext.ValueObjects;
using DirectoryService.Domain.Shared.ValueObjects;

namespace DirectoryService.Domain.PositionContext;

public class Position
{
	public PositionId Id { get; private set; }
	public PositionName Name { get; private set; }
	public PositionDescription Description { get; private set; }
	public IsActive IsActive { get; private set; }

	// Приватный конструктор
	private Position(PositionId id, PositionName name, PositionDescription description, IsActive isActive)
	{
		Id = id;
		Name = name;
		Description = description;
		IsActive = isActive;
	}

	// Фабричный метод для создания новой позиции
	public static Position Create(PositionName name, PositionDescription description, IsActive isActive)
	{
		return new Position(PositionId.Create(), name, description, isActive);
	}

	// Фабричный метод для восстановления из хранилища
	public static Position Restore(PositionId id, PositionName name, PositionDescription description, IsActive isActive)
	{
		return new Position(id, name, description, isActive);
	}

	// Метод для обновления позиции
	public void Update(PositionName name, PositionDescription description, IsActive isActive)
	{
		Name = name;
		Description = description;
		IsActive = isActive;
	}
}
