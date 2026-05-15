using DirectoryService.Domain.PositionContext;
using DirectoryService.Domain.PositionContext.ValueObjects;

namespace DirectoryService.WebApi;

public sealed class InMemoryPosVerification : PosVerification
{
    public InMemoryPosVerification(IEnumerable<Position> existingPositions)
        : base(existingPositions.ToList()) { }

    public new bool CheckUniqueness(Position other)
    {
        return !GetExistingPositions()
            .Any(p =>
                p.Name.Value.Equals(other.Name.Value, StringComparison.OrdinalIgnoreCase)
                && p.LifeTime.IsActivate
            );
    }
}
