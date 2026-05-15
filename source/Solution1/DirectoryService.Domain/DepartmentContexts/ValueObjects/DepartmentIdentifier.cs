using System.Xml.Linq;

namespace DirectoryService.Domain.DepartmentContext.ValueObjects;

public record DepartmentIdentifier
{
	public string Value { get; }

	private DepartmentIdentifier()
	{
		Value = string.Empty;
	}

	private DepartmentIdentifier(string value)
	{
		Value = value;
	}

	private const int MaxLength = 50;

	public static DepartmentIdentifier Create(string identifier)
	{
		if (string.IsNullOrWhiteSpace(identifier))
		{
			throw new ArgumentException("Идентификатор не может быть пустым.", nameof(identifier));
		}

		var trimmed = identifier.Trim();

		if (trimmed.Length > MaxLength)
		{
			throw new ArgumentException($"Идентификатор не может превышать {MaxLength} символов.", nameof(identifier));
		}
		if (!System.Text.RegularExpressions.Regex.IsMatch(trimmed, "^[a-z0-9-]+$"))
		{
			throw new ArgumentException(
				"Идентификатор должен содержать только строчные латинские буквы, цифры и дефисы.",
				nameof(identifier)
			);
		}
		return new DepartmentIdentifier(trimmed);
	}

	public static DepartmentIdentifier Root => Create("root");
}
