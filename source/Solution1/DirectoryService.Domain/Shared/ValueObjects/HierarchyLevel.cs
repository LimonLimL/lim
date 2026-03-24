using System;
using System.Collections.Generic;
using System.Text;

namespace DirectoryService.Domain.DepartmentContext.ValueObjects
{
	public class HierarchyLevel
	{
		public int Value { get; private set; }

		public static HierarchyLevel Create(int value) => new(value);

		private HierarchyLevel(int value) => Value = value;
	}
}
