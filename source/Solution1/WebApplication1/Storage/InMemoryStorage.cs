using System.Threading;
using DirectoryService.Domain.LocationsContext;
using DirectoryService.Domain.LocationsContext.ValueObjects;
using DirectoryService.Domain.PositionContext;
using DirectoryService.Domain.PositionContext.ValueObjects;
using DirectoryService.Domain.Shared.ValueObjects;

namespace WebApplication1.Storage;

public static class Storage
{
    private static readonly Dictionary<LocationId, Location> _locations = new();
    private static readonly Dictionary<PositionId, Position> _positions = new();
    private static object _lock = new();

    public static void Add(Location location)
    {
        if (_locations.ContainsKey(location.Id))
            throw new InvalidOperationException("Локация с таким ID уже существует.");

        if (
            _locations.Any(l =>
                l.Value.Name.Value == location.Name.Value && l.Value.LifeTime.IsActivate
            )
        )
            throw new InvalidOperationException("Локация с таким именем уже существует.");

        _locations.Add(location.Id, location);
    }

    public static Location? GetById(LocationId id)
    {
        if (_locations.TryGetValue(id, out var location))
        {
            return location.LifeTime.IsActivate ? location : null;
        }
        return null;
    }

    public static IEnumerable<Location> GetAllLocations()
    {
        return _locations.Values.Where(l => l.LifeTime.IsActivate);
    }

    public static void Remove(LocationId id)
    {
        lock (_lock)
        {
            if (!_locations.TryGetValue(id, out var location))
                throw new KeyNotFoundException("Локация не найдена.");

            if (!location.LifeTime.IsActivate)
                throw new InvalidOperationException("Локация уже архивирована.");

            var archivedLifeTime = EntityLifeTime.Create(
                location.LifeTime.CreatedAt,
                DateTime.UtcNow,
                DateTime.UtcNow,
                false
            );
        }
    }

    public static bool HardRemove(LocationId id)
    {
        return _locations.Remove(id);
    }

    public static void Add(Position position)
    {
        if (_positions.ContainsKey(position.Id))
            throw new InvalidOperationException("Должность с таким ID уже существует.");

        if (
            _positions.Any(p =>
                p.Value.Name.Value == position.Name.Value && p.Value.LifeTime.IsActivate
            )
        )
            throw new InvalidOperationException("Должность с таким именем уже существует.");

        _positions.Add(position.Id, position);
    }

    public static Position? GetById(PositionId id)
    {
        if (_positions.TryGetValue(id, out var position))
        {
            return position.LifeTime.IsActivate ? position : null;
        }
        return null;
    }

    public static IEnumerable<Position> GetAllPositions()
    {
        return _positions.Values.Where(p => p.LifeTime.IsActivate);
    }

    public static void Remove(PositionId id)
    {
        if (!_positions.TryGetValue(id, out var position))
            throw new KeyNotFoundException("Должность не найдена.");

        if (!position.LifeTime.IsActivate)
            throw new InvalidOperationException("Должность уже архивирована.");

        var archivedLifeTime = EntityLifeTime.Create(
            position.LifeTime.CreatedAt,
            DateTime.UtcNow,
            DateTime.UtcNow,
            false
        );
    }

    public static bool HardRemove(PositionId id)
    {
        return _positions.Remove(id);
    }

    public static void InitializeStorage()
    {
        if (_locations.Count > 0 || _positions.Count > 0)
            return;

        lock (_lock)
        {
            var now = DateTime.UtcNow;

            var location1 = Location.Create(
                Guid.NewGuid(),
                "г. Москва, ул. Тверская, 1",
                "Москва",
                "Europe/Moscow",
                now,
                now
            );
            _locations[location1.Id] = location1;

            var location2 = Location.Create(
                Guid.NewGuid(),
                "г. Санкт-Петербург, Невский проспект, 28",
                "Санкт-Петербург",
                "Europe/Moscow",
                now,
                now
            );
            _locations[location2.Id] = location2;

            var pos1 = new Position("Backend Developer");
            _positions[pos1.Id] = pos1;

            var pos2 = new Position("QA Engineer");
            _positions[pos2.Id] = pos2;
        }
    }
}
