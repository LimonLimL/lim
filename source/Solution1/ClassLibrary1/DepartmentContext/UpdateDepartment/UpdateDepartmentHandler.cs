using DirectoryService.Domain.DepartmentContext.ValueObjects;
using DirectoryService.Domain.DepartmentContexts;

namespace Application.DepartmentContext.UpdateDepartment;

public sealed class UpdateDepartmentHandler
{
    private readonly IDepartmentRepository _repository;

    public UpdateDepartmentHandler(IDepartmentRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(UpdateDepartmentCommand command, CancellationToken ct = default)
    {
        var department = await _repository.GetById(command.Id, ct);
        if (department is null)
            throw new InvalidOperationException($"Отдел с ID {command.Id} не найден.");

        var newName = command.NewName is not null
            ? DepartmentName.Create(command.NewName)
            : department.Name;

        var newIdentifier = command.NewName is not null
            ? DepartmentIdentifier.Create(command.NewName)
            : department.Identifier;

        var isActive = department.LifeTime.IsActivate;
        department.Update(newName, newIdentifier, isActive);

        await _repository.Update(department, ct);
        return department.Id.Value;
    }
}
