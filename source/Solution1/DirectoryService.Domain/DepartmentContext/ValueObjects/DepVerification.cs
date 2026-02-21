using System;
using System.Collections.Generic;
using System.Text;
using DirectoryService.Domain.PositionContext;

namespace DirectoryService.Domain.DepartmentContext.ValueObjects
{
	public interface DepVerification
	{
		public bool СheckUniqueness(Department other);
	}
}
