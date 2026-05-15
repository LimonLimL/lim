using DirectoryService.Domain.DepartmentContexts;
using DirectoryService.Domain.PositionContext;
using DirectoryService.Domain.PositionContext.ValueObjects;

namespace DirectoryService.Domain.PositionContexts.ValueObjects
{
	public class MovePosition
	{
		private readonly List<PositionAdvertisement> _advertisements;

		private MovePosition()
		{
			_advertisements = new List<PositionAdvertisement>();
		}

		public MovePosition(List<PositionAdvertisement> advertisements)
		{
			_advertisements = advertisements;
		}

		public void Execute(PositionId targetId, PositionId relativeId)
		{
			PositionAdvertisement target = FindAdvertisementOrThron(
				targetId,
				errorMessage: "Не найдена должность для перемещения"
			);
			PositionAdvertisement relative = FindAdvertisementOrThron(
				relativeId,
				errorMessage: "Не найдено должность относительно которой происходит перемещение."
			);
			target.Move(_advertisements, relative);
		}

		private PositionAdvertisement FindAdvertisementOrThron(PositionId positionId, string errorMessage)
		{
			return _advertisements.FirstOrDefault(p => p.PositionId == positionId)
				?? throw new InvalidOperationException(errorMessage);
		}
	}
}
