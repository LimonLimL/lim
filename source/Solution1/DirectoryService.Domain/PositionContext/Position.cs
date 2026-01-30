// Domain/PositionContext/Position.cs
using Domain.PositionContext.ValueObjects;
using Domain.Shared.ValueObjects;

namespace Domain.PositionContext;

public class Position
{
    public PositionId Id { get; private set; }
    public PositionName Name { get; private set; }
    public PositionDescription Description { get; private set; }
    public IsActive IsActive { get; private set; }
    public PositionLifeTime LifeTime { get; private set; }

    // Приватный конструктор
    private Position(
        PositionId id,
        PositionName name,
        PositionDescription description,
        IsActive isActive,
        PositionLifeTime lifeTime
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
        IsActive isActive
    )
    {
        return new Position(
            PositionId.Create(),
            name,
            description,
            isActive,
            PositionLifeTime.CreateNew()
        );
    }

    // Фабричный метод для восстановления из хранилища
    public static Position Restore(
        PositionId id,
        PositionName name,
        PositionDescription description,
        IsActive isActive,
        PositionLifeTime lifeTime
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
        LifeTime = LifeTime.Update();
    }
}
