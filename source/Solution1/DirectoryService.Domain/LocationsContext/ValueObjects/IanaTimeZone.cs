public sealed record IanaTimeZone
{
    public string TimeZone { get; }

    private IanaTimeZone(string timeZone) => TimeZone = timeZone;

    public static IanaTimeZone Create(string timeZone)
    {
        if (string.IsNullOrWhiteSpace(timeZone))
            throw new ArgumentException("Часовой пояс не может быть пустым.", nameof(timeZone));
        return new IanaTimeZone(timeZone);
    }
}
