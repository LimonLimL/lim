using DirectoryService.Domain.PositionContext;
using DirectoryService.Domain.PositionContext.ValueObjects;

namespace DirectoryService.WebApi;

public sealed class InMemoryPosVerification : PosVerification
{
    private readonly IEnumerable<Position> _existingPositions;

    public InMemoryPosVerification(IEnumerable<Position> existingPositions)
    {
        _existingPositions = existingPositions;
    }

    public bool CheckUniqueness(Position other)
    {
        return !_existingPositions.Any(p =>
            p.Name.Value.Equals(other.Name.Value, StringComparison.OrdinalIgnoreCase)
            && p.LifeTime.IsActivate
        );
    }
}
