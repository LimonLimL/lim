using System.Xml.Linq;

namespace DirectoryService.Domain.DepartmentContext.ValueObjects;

public record DepartmentPath
{
	public string Value { get; private set; }

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

	public static DepartmentPath СоздатьИзИдентификатора(DepartmentIdentifier identifier)
	{
		return new(identifier.Value);
	}

	internal static DepartmentPath Create(string joinedName)
	{
		throw new NotImplementedException();
	}

	public bool IsRoot => string.IsNullOrEmpty(Value);
}
