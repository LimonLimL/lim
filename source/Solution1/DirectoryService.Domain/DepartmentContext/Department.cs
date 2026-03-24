using System.Xml.Linq;
using DirectoryService.Domain.DepartmentContext.ValueObjects;
using DirectoryService.Domain.LocationsContext.ValueObjects;
using DirectoryService.Domain.PositionContext.ValueObjects;
using DirectoryService.Domain.Shared.ValueObjects;

namespace DirectoryService.Domain.DepartmentContext;

public class Department
{
	public DepartmentId Id { get; private set; }
	public DepartmentName Name { get; private set; }
	public DepartmentIdentifier Identifier { get; private set; }
	public DepartmentId? ParentId { get; private set; }
	public DepartmentPath Path { get; private set; }
	public HierarchyLevel Level { get; private set; }
	public bool IsActive { get; private set; }

	private List<LocInDep> _locInDeps = new();
	private List<PosInDep> _posInDeps = new();

	private Department(
		DepartmentId id,
		DepartmentName name,
		DepartmentIdentifier identifier,
		DepartmentId? parentId,
		DepartmentPath path,
		HierarchyLevel level,
		bool isActive
	)
	{
		Id = id;
		Name = name;
		Identifier = identifier;
		ParentId = parentId;
		Path = path;
		Level = level;
		IsActive = isActive;
	}

	public static Department CreateRoot(DepartmentName name)
	{
		return CreateRoot(
			name,
			DepartmentIdentifier.Create(name.Value),
			new DepVerification(new List<Department>()),
			true
		);
	}

	public static Department CreateRoot(
		DepartmentName name,
		DepartmentIdentifier identifier,
		DepVerification depVerification,
		bool isActive = true
	)
	{
		var department = new Department(
			DepartmentId.Create(),
			name,
			identifier,
			null,
			DepartmentPath.CreateRoot(name.Value),
			HierarchyLevel.Create(1),
			isActive
		);

		bool depVer = depVerification.CheckUniqueness(department);
		if (!depVer)
		{
			throw new InvalidOperationException($"{name} уже существует");
		}

		return department;
	}

	public static Department CreateChild(
		DepartmentName name,
		DepartmentIdentifier identifier,
		Department parent,
		bool isActive = true
	)
	{
		ArgumentNullException.ThrowIfNull(parent);

		var path = DepartmentPath.Create($"{parent.Path.Value}.{name.Value}");
		var level = HierarchyLevel.Create(parent.Level.Value + 1);

		return new Department(DepartmentId.Create(), name, identifier, parent.Id, path, level, isActive);
	}

	public void ConnectDepartment(Department department)
	{
		if (IsSameDepartment(department))
		{
			throw new InvalidOperationException("Подразделение не может быть родителем самого себя.");
		}

		if (IsDescendantOf(department))
		{
			throw new InvalidOperationException("Подразделение не может быть привязано к своему потомку.");
		}

		department.ParentId = Id;
		department.Path = CreateHierarchicalPath(department);
		department.Level = CalculateHierarchyLevel(department);
	}

	private DepartmentPath CreateHierarchicalPath(Department department)
	{
		const char separator = '.';
		string[] names = [Name.Value, department.Name.Value];
		string joinedName = string.Join(separator, names);
		return DepartmentPath.Create(joinedName);
	}

	private HierarchyLevel CalculateHierarchyLevel(Department department)
	{
		const char separator = '.';
		string[] names = department.Path.Value.Split(separator);
		return HierarchyLevel.Create(names.Length);
	}

	private bool IsSameDepartment(Department department)
	{
		return Id == department.Id;
	}

	private bool IsDescendantOf(Department department)
	{
		if (department.ParentId == null)
		{
			return false;
		}
		return department.ParentId == Id;
	}

	public void Update(DepartmentName name, DepartmentIdentifier identifier, bool isActive)
	{
		Name = name;
		Identifier = identifier;
		IsActive = isActive;
	}

	public void AddLoc(LocInDep locInDep)
	{
		foreach (var dep in _locInDeps)
		{
			if (dep.LocationId == locInDep.LocationId)
				throw new InvalidOperationException("ID не может повторяться внутри подразделения");
		}
		_locInDeps.Add(locInDep);
	}

	public void AddPos(PosInDep posInDep)
	{
		foreach (var dep in _posInDeps)
		{
			if (dep.PositionId == posInDep.PositionId)
				throw new InvalidOperationException("Должность не может повторяться внутри подразделения");
		}
		_posInDeps.Add(posInDep);
	}
}
