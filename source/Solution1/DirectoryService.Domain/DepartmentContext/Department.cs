// Domain/DepartmentContext/Department.cs
using Domain.DepartmentContext.ValueObjects;
using Domain.Shared.ValueObjects;

namespace Domain.DepartmentContext;

public class Department
{
    public DepartmentId Id { get; private set; }
    public DepartmentName Name { get; private set; }
    public DepartmentIdentifier Identifier { get; private set; }
    public DepartmentId? ParentId { get; private set; }
    public DepartmentPath Path { get; private set; }
    public DepartmentDepth Depth { get; private set; }
    public IsActive IsActive { get; private set; }
    public DepartmentLifeTime LifeTime { get; private set; }

    // Приватный конструктор для внутреннего использования
    private Department(
        DepartmentId id,
        DepartmentName name,
        DepartmentIdentifier identifier,
        DepartmentId? parentId,
        DepartmentPath path,
        DepartmentDepth depth,
        IsActive isActive,
        DepartmentLifeTime lifeTime
    )
    {
        Id = id;
        Name = name;
        Identifier = identifier;
        ParentId = parentId;
        Path = path;
        Depth = depth;
        IsActive = isActive;
        LifeTime = lifeTime;
    }

    // Фабричный метод для создания корневого подразделения
    public static Department CreateRoot(
        DepartmentName name,
        DepartmentIdentifier identifier,
        IsActive isActive
    )
    {
        return new Department(
            DepartmentId.Create(),
            name,
            identifier,
            null,
            DepartmentPath.CreateRoot(),
            DepartmentDepth.Root,
            isActive,
            DepartmentLifeTime.CreateNew()
        );
    }

    // Фабричный метод для создания дочернего подразделения
    public static Department CreateChild(
        DepartmentName name,
        DepartmentIdentifier identifier,
        Department parent,
        IsActive isActive
    )
    {
        if (parent == null)
            throw new ArgumentNullException(nameof(parent));

        return new Department(
            DepartmentId.Create(),
            name,
            identifier,
            parent.Id,
            DepartmentPath.Create(parent.Path, identifier),
            parent.Depth.Increment(),
            isActive,
            DepartmentLifeTime.CreateNew()
        );
    }

    // Метод для обновления подразделения
    public void Update(DepartmentName name, DepartmentIdentifier identifier, IsActive isActive)
    {
        Name = name;
        Identifier = identifier;
        IsActive = isActive;
        LifeTime = LifeTime.Update();

        // Примечание: изменение иерархии (ParentId, Path, Depth) требует отдельной логики перемещения в дереве
    }
}
