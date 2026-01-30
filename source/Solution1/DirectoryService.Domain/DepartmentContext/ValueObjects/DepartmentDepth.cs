// Domain/DepartmentContext/ValueObjects/DepartmentDepth.cs
namespace Domain.DepartmentContext.ValueObjects;

public record DepartmentDepth(short Value)
{
    public static DepartmentDepth Create(short depth)
    {
        if (depth < 0)
            throw new ArgumentException("Depth cannot be negative", nameof(depth));

        return new DepartmentDepth(depth);
    }

    public static DepartmentDepth Root => Create(0);

    public DepartmentDepth Increment() => Create((short)(Value + 1));
}
