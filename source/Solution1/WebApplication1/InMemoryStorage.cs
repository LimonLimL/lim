using DirectoryService.Domain.LocationsContext;
using DirectoryService.Domain.LocationsContext.ValueObjects;
using DirectoryService.Domain.PositionContext;
using DirectoryService.Domain.PositionContext.ValueObjects;
using DirectoryService.Domain.Shared.ValueObjects;

namespace DirectoryService.WebApplication; 

public static class InMemoryStorage
{
    
    private static readonly Dictionary<LocationId, Location> _locations = new();
    private static readonly Dictionary<PositionId, Position> _positions = new();

   

    public static void AddLocation(Location location)
    {
        if (_locations.ContainsKey(location.Id))
            throw new InvalidOperationException($"Location with ID {location.Id} already exists.");

        if (_locations.Any(l => l.Value.Name.Value == location.Name.Value))
            throw new InvalidOperationException($"Location with Name '{location.Name.Value}' already exists.");

        _locations.Add(location.Id, location);
    }

    public static Location? GetLocationById(LocationId id)
    {
        _locations.TryGetValue(id, out var location);
        return location;
    }

    public static IEnumerable<Location> GetAllLocations()
    {
        return _locations.Values;
    }

    public static void ArchiveLocation(LocationId id)
    {
        if (_locations.TryGetValue(id, out var location))
        {
            location.LifeTime = EntityLifeTime.Create(location.LifeTime.CreatedAt, location.LifeTime.UpdatedAt, false);
        }
        else
        {
            throw new InvalidOperationException("Location not found.");
        }
    }

    public static void HardRemoveLocation(LocationId id)
    {
        _locations.Remove(id);
    }


    public static void AddPosition(Position position)
    {
        if (_positions.ContainsKey(position.Id))
            throw new InvalidOperationException($"Position with ID {position.Id} already exists.");

        if (_positions.Any(p => p.Value.Name.Value == position.Name.Value))
            throw new InvalidOperationException($"Position with Name '{position.Name.Value}' already exists.");

        _positions.Add(position.Id, position);
    }

    public static Position? GetPositionById(PositionId id)
    {
        _positions.TryGetValue(id, out var position);
        return position;
    }

    public static IEnumerable<Position> GetAllPositions()
    {
        return _positions.Values;
    }

    public static void ArchivePosition(PositionId id)
    {
        if (_positions.TryGetValue(id, out var position))
        {
            position.LifeTime = 
        }
        else
        {
            throw new InvalidOperationException("Position not found.");
        }
    }

    public static void HardRemovePosition(PositionId id)
    {
        _positions.Remove(id);
    }

    public static void InitializeStorage()
    {
        
        var loc1 = new Location(new LocationName("Москва"), new LocationDescription("Столица"), IsActive.Create(true), EntityLifeTime.CreateNew());
        AddLocation(new Location(LocationId.Create(), new LocationName("Москва"), ...));
        AddLocation(new Location(LocationId.Create(), new LocationName("Санкт-Петербург"), ...));   
        AddPosition(new Position(PositionId.Create(), new PositionName("Директор"), ...));
        AddPosition(new Position(PositionId.Create(), new PositionName("Разработчик"), ...));
       
    }
}