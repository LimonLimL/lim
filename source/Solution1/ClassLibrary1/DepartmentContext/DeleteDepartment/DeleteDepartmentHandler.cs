public sealed class DeleteDepartmentHandler
{
    private readonly IDepartmentRepository _repository;

    public DeleteDepartmentHandler(IDepartmentRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(DeleteDepartmentCommand command, CancellationToken ct = default)
    {
        var department = await _repository.GetById(command.Id, ct);

        if (department is null)
        {
            throw new InvalidOperationException("Отдел не найден.");
        }

        await _repository.Delete(department, ct);

        return department.Id.Value;
    }
}
