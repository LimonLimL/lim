// Domain/DepartmentContext/ValueObjects/DepartmentDepth.cs
namespace DirectoryService.Domain.DepartmentContext.ValueObjects;

/// <summary>
/// Представляет глубину отдела в иерархической структуре.
/// </summary>
public record DepartmentDepth
{
	public short Value { get; }

	private DepartmentDepth(short value)
	{
		Value = value;
	}

	/// <summary>
	/// Создает новый экземпляр <see cref="DepartmentDepth"/> с указанным значением глубины.
	/// </summary>
	/// <param name="depth">Значение глубины, должно быть неотрицательным.</param>
	/// <returns>Новый экземпляр <see cref="DepartmentDepth"/>.</returns>
	/// <exception cref="ArgumentException">Выбрасывается, если <paramref name="depth"/> отрицательный.</exception>
	public static DepartmentDepth Create(short depth)
	{
		if (depth < 0)
		{
			throw new ArgumentException("Глубина не может быть отрицательной.", nameof(depth));
		}

		return new DepartmentDepth(depth);
	}

	/// <summary>
	/// Получает корневую глубину отдела, которая имеет значение 0.
	/// </summary>
	public static DepartmentDepth Root => Create(0);

	/// <summary>
	/// Увеличивает текущую глубину отдела на 1.
	/// </summary>
	/// <returns>Новый экземпляр <see cref="DepartmentDepth"/> с увеличенным значением.</returns>
	public DepartmentDepth Increment()
	{
		return Create((short)(Value + 1));
	}
}
