using DirectoryService.Domain.DepartmentContext.ValueObjects;
using Xunit;

namespace DirectoryService.Domain.Tests.DepartmentContext.ValueObjects;

public class DepartmentIdentifierTests
{
    [Theory]
    [InlineData("root")]
    [InlineData("department-1")]
    [InlineData("test123")]
    [InlineData("a")]
    [InlineData("abcdefghijklmnopqrstuvwxyz0123456789-")]
    [InlineData("valid-identifier-with-numbers-123")]
    public void Create_ValidIdentifier_Success(string identifier)
    {
        var result = DepartmentIdentifier.Create(identifier);

        Assert.NotNull(result);
        Assert.Equal(identifier.Trim(), result.Value);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("\t")]
    [InlineData("\n")]
    public void Create_NullOrWhitespace_ThrowsArgumentException(string identifier)
    {
        var exception = Assert.Throws<ArgumentException>(() =>
            DepartmentIdentifier.Create(identifier)
        );
        Assert.Equal("identifier", exception.ParamName);
        Assert.Contains("Идентификатор не может быть пустым", exception.Message);
    }

    [Fact]
    public void Create_ExceedsMaxLength_ThrowsArgumentException()
    {
        var longIdentifier = new string('a', 51);

        var exception = Assert.Throws<ArgumentException>(() =>
            DepartmentIdentifier.Create(longIdentifier)
        );
        Assert.Equal("identifier", exception.ParamName);
        Assert.Contains("Идентификатор не может превышать 50 символов", exception.Message);
    }

    [Theory]
    [InlineData("ABC")]
    [InlineData("Test")]
    [InlineData("test_123")]
    [InlineData("test@123")]
    [InlineData("test 123")]
    [InlineData("test.123")]
    [InlineData("тест123")]
    [InlineData("test123!")]
    [InlineData("test#123")]
    public void Create_InvalidCharacters_ThrowsArgumentException(string identifier)
    {
        var exception = Assert.Throws<ArgumentException>(() =>
            DepartmentIdentifier.Create(identifier)
        );
        Assert.Equal("identifier", exception.ParamName);
        Assert.Contains(
            "Идентификатор должен содержать только строчные латинские буквы, цифры и дефисы",
            exception.Message
        );
    }

    [Fact]
    public void Root_ReturnsRootIdentifier()
    {
        var result = DepartmentIdentifier.Root;

        Assert.NotNull(result);
        Assert.Equal("root", result.Value);
    }

    [Theory]
    [InlineData("  test  ")]
    [InlineData("   root")]
    [InlineData("department-1   ")]
    public void Create_TrimsWhitespace_Success(string identifier)
    {
        var result = DepartmentIdentifier.Create(identifier);

        Assert.NotNull(result);
        Assert.Equal(identifier.Trim(), result.Value);
    }

    [Fact]
    public void Create_MaxLengthExactly_Success()
    {
        var maxLengthIdentifier = "abcdefghijklmnopqrstuvwxyz0123456789--------------";

        var result = DepartmentIdentifier.Create(maxLengthIdentifier);

        Assert.NotNull(result);
        Assert.Equal(maxLengthIdentifier, result.Value);
        Assert.Equal(50, result.Value.Length);
    }
}
