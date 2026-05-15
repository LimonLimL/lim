namespace DirectoryService.Domain.DepartmentContext.ValueObjects
{
	public record HierarchyLevel
	{
		public int Value { get; }

		private HierarchyLevel() { }

		private HierarchyLevel(int value)
		{
			if (value < 0)
				throw new ArgumentException("Level cannot be negative", nameof(value));

			Value = value;
		}

		public static HierarchyLevel Create(int value) => new(value);
	}
}
