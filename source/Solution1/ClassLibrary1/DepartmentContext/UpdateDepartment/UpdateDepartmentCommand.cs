public sealed record UpdateDepartmentCommand(Guid Id, string? NewName, Guid? NewParentId);
