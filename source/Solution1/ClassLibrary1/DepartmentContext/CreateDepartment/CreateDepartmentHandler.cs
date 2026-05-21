using DirectoryService.Domain.DepartmentContext.ValueObjects;
using DirectoryService.Domain.DepartmentContexts;
using DirectoryService.Domain.Shared.ValueObjects;

namespace Application.DepartmentContext.CreateDepartment;

public sealed class CreateDepartmentHandler
{
    private readonly IDepartmentRepository _repository;

    public CreateDepartmentHandler(IDepartmentRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(CreateDepartmentCommand command, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(command.Name))
            throw new ArgumentException(
                "Название отдела не может быть пустым.",
                nameof(command.Name)
            );

        var name = DepartmentName.Create(command.Name);

        var identifierValue = CreateSlug(command.Name);
        var identifier = DepartmentIdentifier.Create(identifierValue);

        var exists = await _repository.Exists(identifierValue, ct);
        if (exists)
            throw new InvalidOperationException(
                $"Отдел с идентификатором '{identifierValue}' уже существует."
            );

        Department department;

        if (command.ParentId.HasValue)
        {
            var parent = await _repository.GetById(command.ParentId.Value, ct);
            if (parent is null)
                throw new InvalidOperationException("Родительский отдел не найден.");

            department = Department.CreateChild(name, identifier, parent);
        }
        else
        {
            department = Department.CreateRoot(
                name,
                identifier,
                new DepVerification(new List<Department>())
            );
        }

        await _repository.Add(department, ct);
        return department.Id.Value;
    }

    private static string CreateSlug(string name)
    {
        return name.ToLowerInvariant().Replace(" ", "-").Replace("_", "-").Trim('-');
    }
}
