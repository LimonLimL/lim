using DirectoryService.Domain.DepartmentContext;
using DirectoryService.Domain.DepartmentContext.ValueObjects;
using DirectoryService.Domain.DepartmentContexts;
using Inrstructure.PostgreSQL;
using Microsoft.EntityFrameworkCore;

public sealed class DepartmentRepository : IDepartmentRepository
{
    private readonly ApplicationDbContext _context;

    public DepartmentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Department?> GetById(Guid id, CancellationToken ct = default)
    {
        var deptId = DepartmentId.From(id);
        return await _context.Departments.FirstOrDefaultAsync(d => d.Id == deptId, ct);
    }

    public async Task<IEnumerable<Department>> GetAll(CancellationToken ct = default)
    {
        return await _context.Departments.ToListAsync(ct);
    }

    public async Task<bool> Exists(string name, CancellationToken ct = default)
    {
        var deptName = DepartmentName.Create(name);
        return await _context.Departments.AnyAsync(d => d.Name == deptName, ct);
    }

    public async Task<Department?> GetByName(DepartmentName name, CancellationToken ct = default)
    {
        return await _context.Departments.FirstOrDefaultAsync(d => d.Name == name, ct);
    }

    public async Task Add(Department department, CancellationToken ct = default)
    {
        await _context.Departments.AddAsync(department, ct);
        await _context.SaveChangesAsync(ct);
    }

    public async Task Update(Department department, CancellationToken ct = default)
    {
        _context.Departments.Update(department);
        await _context.SaveChangesAsync(ct);
    }

    public async Task Delete(Department department, CancellationToken ct = default)
    {
        _context.Departments.Remove(department);
        await _context.SaveChangesAsync(ct);
    }
}
