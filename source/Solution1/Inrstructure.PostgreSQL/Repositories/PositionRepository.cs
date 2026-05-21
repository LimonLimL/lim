using DirectoryService.Domain.PositionContext;
using DirectoryService.Domain.PositionContext.ValueObjects;
using DirectoryService.Domain.PositionContexts.Contracts;
using Inrstructure.PostgreSQL;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.PostgreSQL.Repositories;

public sealed class PositionRepository : IPositionRepository
{
    private readonly ApplicationDbContext _context;

    public PositionRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Exists(string name, CancellationToken ct = default)
    {
        var positionName = PositionName.Create(name);
        return await _context.Positions.AnyAsync(p => p.Name == positionName, ct);
    }

    public async Task Add(Position position, CancellationToken ct = default)
    {
        await _context.Positions.AddAsync(position, ct);
        await _context.SaveChangesAsync(ct);
    }

    public async Task<Position?> GetById(Guid id, CancellationToken ct = default)
    {
        var positionId = PositionId.From(id);
        return await _context.Positions.FirstOrDefaultAsync(p => p.Id == positionId, ct);
    }

    public async Task<Position?> GetByName(PositionName name, CancellationToken ct = default)
    {
        return await _context.Positions.FirstOrDefaultAsync(p => p.Name == name, ct);
    }

    public async Task<IEnumerable<Position>> GetAll(CancellationToken ct = default)
    {
        return await _context.Positions.ToListAsync(ct);
    }

    public async Task Update(Position position, CancellationToken ct = default)
    {
        _context.Positions.Update(position);
        await _context.SaveChangesAsync(ct);
    }

    public async Task<IEnumerable<Position>> GetManyByIds(
        IEnumerable<Guid> ids,
        CancellationToken ct = default
    )
    {
        var positionIds = ids.Select(id => PositionId.From(id)).ToList();
        return await _context.Positions.Where(p => positionIds.Contains(p.Id)).ToListAsync(ct);
    }

    public async Task DeleteMany(IEnumerable<Position> positions, CancellationToken ct = default)
    {
        _context.Positions.RemoveRange(positions);
        await _context.SaveChangesAsync(ct);
    }
}
