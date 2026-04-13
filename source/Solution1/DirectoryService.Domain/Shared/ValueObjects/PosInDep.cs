using DirectoryService.Domain.DepartmentContext.ValueObjects;
using DirectoryService.Domain.DepartmentContexts;
using DirectoryService.Domain.PositionContext;
using DirectoryService.Domain.PositionContext.ValueObjects;

namespace DirectoryService.Domain.Shared.ValueObjects
{
	public sealed class PosInDep
	{
		public PosInDep(PositionId positionId)
		{
			PositionId = positionId;
		}

		public PosInDep(Position position, Department department)
		{
			Position = position;
			Department = department;
			PositionId = position.Id;
			DepartmentId = department.Id;
		}

		public PositionId PositionId { get; }
		public Position? Position { get; }
		public DepartmentId? DepartmentId { get; }
		public Department? Department { get; }
	}
}
