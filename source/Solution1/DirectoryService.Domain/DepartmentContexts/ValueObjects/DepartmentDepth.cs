namespace DirectoryService.Domain.DepartmentContext.ValueObjects;

public record DepartmentDepth
{
	public short Value { get; }

	private DepartmentDepth(short value)
	{
		Value = value;
	}

	public static DepartmentDepth Create(short depth)
	{
		if (depth < 0)
		{
			throw new ArgumentException("Глубина не может быть отрицательной.", nameof(depth));
		}

		return new DepartmentDepth(depth);
	}

	public static DepartmentDepth Root => Create(0);

	public DepartmentDepth Increment()
	{
		return Create((short)(Value + 1));
	}
}
