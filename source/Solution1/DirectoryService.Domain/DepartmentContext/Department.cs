// Domain/DepartmentContext/Department.cs

// Domain/DepartmentContext/Department.cs
using DirectoryService.Domain.DepartmentContext.ValueObjects;

namespace DirectoryService.Domain.DepartmentContext;

/// <summary>
/// Сущность отдела в домене.
/// Содержит идентификатор, название, идентификатор родителя, путь, глубину, статус активности и временные метки.
/// Обеспечивает инварианты через фабричные методы: корень и дочерние отделы создаются с соблюдением иерархии.
/// </summary>
public class Department
{
	/// <summary>
	/// Идентификатор отдела.
	/// </summary>
	public DepartmentId Id { get; private set; }

	/// <summary>
	/// Название отдела.
	/// </summary>
	public DepartmentName Name { get; private set; }

	/// <summary>
	/// Уникальный текстовый идентификатор отдела (slug).
	/// </summary>
	public DepartmentIdentifier Identifier { get; private set; }

	/// <summary>
	/// Идентификатор родительского отдела (может быть null для корня).
	/// </summary>
	public DepartmentId? ParentId { get; private set; }

	/// <summary>
	/// Путь к отделу в иерархии (например, "it/hr").
	/// </summary>
	public DepartmentPath Path { get; private set; }

	/// <summary>
	/// Глубина отдела в дереве (0 — корень, 1 — дочерний и т.д.).
	/// </summary>
	public DepartmentDepth Depth { get; private set; }

	/// <summary>
	/// Признак активности отдела.
	/// </summary>
	public bool IsActive { get; private set; }

	/// <summary>
	/// Внутренний конструктор для создания экземпляра отдела.
	/// Используется только фабричными методами.
	/// </summary>
	/// <param name="id">Идентификатор отдела.</param>
	/// <param name="name">Название отдела.</param>
	/// <param name="identifier">Текстовый идентификатор.</param>
	/// <param name="parentId">Идентификатор родителя (null для корня).</param>
	/// <param name="path">Путь в иерархии.</param>
	/// <param name="depth">Глубина в дереве.</param>
	/// <param name="isActive">Активность отдела.</param>
	/// <param name="lifeTime">Временные метки.</param>
	private Department(
		DepartmentId id,
		DepartmentName name,
		DepartmentIdentifier identifier,
		DepartmentId? parentId,
		DepartmentPath path,
		DepartmentDepth depth,
		bool isActive
	)
	{
		Id = id;
		Name = name;
		Identifier = identifier;
		ParentId = parentId;
		Path = path;
		Depth = depth;
		IsActive = isActive;
	}

	/// <summary>
	/// Создаёт корневой отдел (без родителя).
	/// </summary>
	/// <param name="name">Название корневого отдела.</param>
	/// <param name="identifier">Текстовый идентификатор корневого отдела.</param>
	/// <param name="isActive">Активность отдела (по умолчанию — true).</param>
	/// <returns>Новый экземпляр <see cref="Department"/>.</returns>
	public static Department CreateRoot(DepartmentName name, DepartmentIdentifier identifier, bool isActive = true)
	{
		return new Department(
			DepartmentId.Create(),
			name,
			identifier,
			null,
			DepartmentPath.CreateRoot(),
			DepartmentDepth.Create(0),
			isActive
		);
	}

	/// <summary>
	/// Создаёт дочерний отдел под указанным родителем.
	/// Автоматически вычисляет путь и глубину.
	/// </summary>
	/// <param name="name">Название дочернего отдела.</param>
	/// <param name="identifier">Текстовый идентификатор дочернего отдела.</param>
	/// <param name="parent">Родительский отдел (не может быть null).</param>
	/// <param name="isActive">Активность отдела (по умолчанию — true).</param>
	/// <returns>Новый экземпляр <see cref="Department"/>.</returns>
	/// <exception cref="ArgumentNullException">
	/// Выбрасывается, если <paramref name="parent"/> равен null.
	/// </exception>
	public static Department CreateChild(
		DepartmentName name,
		DepartmentIdentifier identifier,
		Department parent,
		bool isActive = true
	)
	{
		if (parent is null)
		{
			ArgumentNullException.ThrowIfNull(parent);
		}

		return new Department(
			DepartmentId.Create(),
			name,
			identifier,
			parent.Id,
			DepartmentPath.Create(parent.Path, identifier),
			parent.Depth.Increment(), // ← Предполагаем, что DepartmentDepth имеет метод Increment()
			isActive
		);
	}

	/// <summary>
	/// Обновляет данные отдела (название, идентификатор, активность).
	/// Изменение иерархии (ParentId, Path, Depth) требует отдельной логики перемещения в дереве.
	/// </summary>
	/// <param name="name">Новое название.</param>
	/// <param name="identifier">Новый текстовый идентификатор.</param>
	/// <param name="isActive">Новый статус активности.</param>
	public void Update(DepartmentName name, DepartmentIdentifier identifier, bool isActive)
	{
		Name = name;
		Identifier = identifier;
		IsActive = isActive;
	}
}
