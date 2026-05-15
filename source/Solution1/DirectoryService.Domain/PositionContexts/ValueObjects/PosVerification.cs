using System;
using System.Collections.Generic;
using System.Text;
using DirectoryService.Domain.PositionContext;

public class PosVerification
{
	private readonly List<Position> _existingPositions;

	private PosVerification()
	{
		_existingPositions = new List<Position>();
	}

	public PosVerification(List<Position> existingPositions)
	{
		_existingPositions = existingPositions ?? new List<Position>();
	}

	protected IEnumerable<Position> GetExistingPositions()
	{
		return _existingPositions;
	}

	public bool CheckUniqueness(Position other)
	{
		if (other == null)
			return false;
		return !_existingPositions.Any(p => p.Name.Value == other.Name.Value && p.Id != other.Id);
	}
}
