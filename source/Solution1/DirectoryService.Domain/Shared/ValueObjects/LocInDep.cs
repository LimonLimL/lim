using DirectoryService.Domain.DepartmentContext.ValueObjects;
using DirectoryService.Domain.DepartmentContexts;
using DirectoryService.Domain.LocationsContext;
using DirectoryService.Domain.LocationsContext.ValueObjects;

namespace DirectoryService.Domain.Shared.ValueObjects
{
	public sealed class LocInDep
	{
		private LocInDep() { }

		public LocInDep(LocationId locationId)
		{
			LocationId = locationId;
		}

		public LocInDep(Location location, Department department)
		{
			Location = location;
			Department = department;
			LocationId = location.Id;
			DepartmentId = department.Id;
		}

		public LocationId LocationId { get; } = null!;
		public Location? Location { get; }
		public DepartmentId? DepartmentId { get; }
		public Department? Department { get; }
	}
}
