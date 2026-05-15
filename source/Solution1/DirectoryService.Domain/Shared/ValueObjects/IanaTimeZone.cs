namespace DirectoryService.Domain.Shared.ValueObjects;

public sealed record IanaTimeZone
{
	public string TimeZone { get; } = null!;

	private IanaTimeZone() { }

	private IanaTimeZone(string timeZone)
	{
		if (string.IsNullOrWhiteSpace(timeZone))
		{
			throw new ArgumentException("Часовой пояс не может быть пустым.", nameof(timeZone));
		}

		TimeZone = timeZone;
	}

	public static IanaTimeZone Create(string timeZone)
	{
		if (string.IsNullOrWhiteSpace(timeZone))
		{
			throw new ArgumentException("Часовой пояс не может быть пустым.", nameof(timeZone));
		}

		return new IanaTimeZone(timeZone);
	}
}
