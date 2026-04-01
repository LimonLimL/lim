using System;
using System.Collections.Generic;
using System.Text;

namespace DirectoryService.Domain.PositionContext.ValueObjects
{
	public interface PosVerification
	{
		public bool СheckUniqueness(Position other);
	}
}
