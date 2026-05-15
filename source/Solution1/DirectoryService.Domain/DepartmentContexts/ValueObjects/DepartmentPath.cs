using System.Xml.Linq;

namespace DirectoryService.Domain.DepartmentContext.ValueObjects;

public record DepartmentPath
{
	public string Value { get; private set; }

	private DepartmentPath()
	{
		Value = string.Empty;
	}

	private DepartmentPath(string value)
	{
		Value = value;
	}

	private const int MaxLength = 255;

	public static DepartmentPath CreateRoot(string value)
	{
		return new(string.Empty);
	}

	public static DepartmentPath Create(DepartmentPath parentPath, DepartmentIdentifier identifier)
	{
		string newValue = string.IsNullOrEmpty(parentPath.Value)
			? identifier.Value
			: $"{parentPath.Value}.{identifier.Value}";

		if (!System.Text.RegularExpressions.Regex.IsMatch(newValue, @"^[a-z0-9\-\.\/]+$"))
		{
			throw new ArgumentException(
				"Путь должен содержать только строчные латинские буквы, цифры, дефисы, точки и слеши",
				nameof(parentPath)
			);
		}

		var trimmed = newValue.Trim();

		if (trimmed.Length > MaxLength)
		{
			throw new ArgumentException($"Путь не может превышать {MaxLength} символов", nameof(parentPath));
		}

		return new DepartmentPath(trimmed);
	}

	public static DepartmentPath Create(string value)
	{
		if (string.IsNullOrWhiteSpace(value))
			throw new ArgumentException("Путь не может быть пустым.", nameof(value));

		var trimmed = value.Trim();

		if (trimmed.Length > MaxLength)
		{
			throw new ArgumentException($"Путь не может превышать {MaxLength} символов", nameof(value));
		}

		return new DepartmentPath(trimmed);
	}

	public bool IsRoot => string.IsNullOrEmpty(Value);
}
