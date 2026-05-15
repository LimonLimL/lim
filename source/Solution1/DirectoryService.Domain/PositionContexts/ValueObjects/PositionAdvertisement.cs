using DirectoryService.Domain.PositionContext.ValueObjects;

namespace DirectoryService.Domain.PositionContext;

public class PositionAdvertisement
{
	private Position? position;

	public PositionId PositionId { get; private set; } = null!;
	public string Name { get; private set; } = null!;
	public Rank Rank { get; private set; } = null!;
	public Position? Position => position;

	private PositionAdvertisement() { }

	public PositionAdvertisement(PositionId positionId, string name, Rank rank)
	{
		PositionId = positionId ?? throw new ArgumentNullException(nameof(positionId));
		Name = name ?? throw new ArgumentNullException(nameof(name));
		Rank = rank ?? throw new ArgumentNullException(nameof(rank));
	}

	public PositionAdvertisement(Position position)
	{
		this.position = position ?? throw new ArgumentNullException(nameof(position));
		PositionId = position.Id;
		Name = position.Name.Value;
		Rank = Rank.Initial();
	}

	public override bool Equals(object? obj)
	{
		if (obj is not PositionAdvertisement other)
			return false;
		return PositionId.Equals(other.PositionId);
	}

	public override int GetHashCode() => PositionId.GetHashCode();

	public void Move(IList<PositionAdvertisement> advertisements, PositionAdvertisement relative)
	{
		if (advertisements == null)
			throw new ArgumentNullException(nameof(advertisements));
		if (relative == null)
			throw new ArgumentNullException(nameof(relative));
		if (!advertisements.Contains(this))
			throw new InvalidOperationException("Перемещаемая должность отсутствует в списке");
		if (!advertisements.Contains(relative))
			throw new InvalidOperationException("Целевая должность отсутствует в списке");

		var currentRank = this.Rank.Value;
		var targetRank = relative.Rank.Value;

		if (currentRank == targetRank)
			return;

		if (currentRank < targetRank)
		{
			foreach (var ad in advertisements)
			{
				if (ad.Rank.Value > currentRank && ad.Rank.Value <= targetRank)
					ad.Rank = Rank.Create(ad.Rank.Value - 1);
			}
			this.Rank = Rank.Create(targetRank);
		}
		else
		{
			foreach (var ad in advertisements)
			{
				if (ad.Rank.Value >= targetRank && ad.Rank.Value < currentRank)
					ad.Rank = Rank.Create(ad.Rank.Value + 1);
			}
			this.Rank = Rank.Create(targetRank);
		}
	}
}
