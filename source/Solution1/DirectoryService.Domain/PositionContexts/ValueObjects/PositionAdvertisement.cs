using DirectoryService.Domain.PositionContext;
using DirectoryService.Domain.PositionContext.ValueObjects;

public class PositionAdvertisement
{
	private Position? position;

	public PositionId PositionId { get; private set; } = null!;
	public string Name { get; private set; } = null!;
	public int Priority { get; private set; }
	public Position? Position => position;

	private PositionAdvertisement() { }

	public PositionAdvertisement(PositionId positionId, string name, int priority = 0)
	{
		PositionId = positionId ?? throw new ArgumentNullException(nameof(positionId));
		Name = name ?? throw new ArgumentNullException(nameof(name));
		Priority = priority;
	}

	public PositionAdvertisement(Position position)
	{
		this.position = position ?? throw new ArgumentNullException(nameof(position));
		PositionId = position.Id;
		Name = position.Name.Value;
	}

	public override bool Equals(object? obj)
	{
		if (obj is not PositionAdvertisement other)
			return false;

		return PositionId.Equals(other.PositionId);
	}

	public override int GetHashCode()
	{
		return PositionId.GetHashCode();
	}

	public void Move(List<PositionAdvertisement> advertisements, PositionAdvertisement relative)
	{
		if (advertisements == null)
			throw new ArgumentNullException(nameof(advertisements));

		if (relative == null)
			throw new ArgumentNullException(nameof(relative));

		if (!advertisements.Remove(this))
			throw new InvalidOperationException("Не удалось удалить должность из списка");

		int relativeIndex = advertisements.IndexOf(relative);
		if (relativeIndex == -1)
			throw new InvalidOperationException("Относительная должность не найдена в списке");

		advertisements.Insert(relativeIndex + 1, this);
	}
}
