public sealed record CreateDepartmentCommand(string Name, Guid? ParentId = null);
