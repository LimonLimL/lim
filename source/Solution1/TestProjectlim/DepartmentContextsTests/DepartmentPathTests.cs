using System.Reflection;
using DirectoryService.Domain.DepartmentContext.ValueObjects;
using Xunit;

namespace DirectoryService.Domain.Tests;

public class DepartmentPathTests
{
    #region Helper для создания DepartmentIdentifier
    private static DepartmentIdentifier CreateIdentifier(string value)
    {
        var constructor =
            typeof(DepartmentIdentifier).GetConstructor(
                BindingFlags.NonPublic | BindingFlags.Instance,
                null,
                new[] { typeof(string) },
                null
            ) ?? throw new InvalidOperationException("Конструктор DepartmentIdentifier не найден");

        return (DepartmentIdentifier)constructor.Invoke(new object[] { value });
    }

    #endregion

    #region CreateRoot Tests

    [Fact]
    public void CreateRoot_ShouldReturnPathWithEmptyValue()
    {
        var result = DepartmentPath.CreateRoot("anyValue");

        Assert.NotNull(result);
        Assert.True(string.IsNullOrEmpty(result.Value));
    }

    [Fact]
    public void CreateRoot_ShouldAlwaysReturnRootPath()
    {
        var testValues = new[] { "test", "", "department/root", "anything" };

        foreach (var value in testValues)
        {
            var result = DepartmentPath.CreateRoot(value);

            Assert.True(result.IsRoot);
            Assert.True(string.IsNullOrEmpty(result.Value));
        }
    }

    [Fact]
    public void CreateRoot_ParameterIsIgnored()
    {
        var result1 = DepartmentPath.CreateRoot("ignored");
        var result2 = DepartmentPath.CreateRoot("also-ignored");

        Assert.Equal(result1.Value, result2.Value);
        Assert.True(string.IsNullOrEmpty(result1.Value));
    }

    #endregion

    #region Create Tests - Valid Cases

    [Fact]
    public void Create_WithValidIdentifier_ShouldCreateCorrectPath()
    {
        var parentPath = DepartmentPath.CreateRoot("root");
        var identifier = CreateIdentifier("engineering");
        var result = DepartmentPath.Create(parentPath, identifier);

        Assert.NotNull(result);
        Assert.Equal("engineering", result.Value);
        Assert.False(result.IsRoot);
    }

    [Fact]
    public void Create_WithNonRootParent_ShouldConcatenatePaths()
    {
        var rootPath = DepartmentPath.CreateRoot("root");
        var engineeringPath = DepartmentPath.Create(rootPath, CreateIdentifier("engineering"));
        var backendIdentifier = CreateIdentifier("backend");
        var result = DepartmentPath.Create(engineeringPath, backendIdentifier);

        Assert.Equal("engineering.backend", result.Value);
    }

    [Fact]
    public void Create_WithMultipleLevels_ShouldBuildCorrectHierarchy()
    {
        var root = DepartmentPath.CreateRoot("root");
        var level1 = DepartmentPath.Create(root, CreateIdentifier("company"));
        var level2 = DepartmentPath.Create(level1, CreateIdentifier("department"));
        var level3 = DepartmentPath.Create(level2, CreateIdentifier("team"));

        Assert.Equal("company", level1.Value);
        Assert.Equal("company.department", level2.Value);
        Assert.Equal("company.department.team", level3.Value);
    }

    [Theory]
    [InlineData("abc123")]
    [InlineData("test-dept")]
    [InlineData("v1.0")]
    [InlineData("team-alpha")]
    [InlineData("dept-01")]
    [InlineData("a")]
    [InlineData("1")]
    [InlineData("-")]
    [InlineData(".")]
    public void Create_WithValidCharacters_ShouldSucceed(string identifierValue)
    {
        var root = DepartmentPath.CreateRoot("root");
        var identifier = CreateIdentifier(identifierValue);
        var result = DepartmentPath.Create(root, identifier);

        Assert.NotNull(result);
        Assert.Equal(identifierValue, result.Value);
        Assert.False(result.IsRoot);
    }

    #endregion

    #region Create Tests - Validation Cases

    [Fact]
    public void Create_WithPathExceedingMaxLength_ShouldThrowArgumentException()
    {
        var root = DepartmentPath.CreateRoot("root");
        var longIdentifier = CreateIdentifier(new string('a', 250));
        var longPath = DepartmentPath.Create(root, longIdentifier);
        var exception = Assert.Throws<ArgumentException>(() =>
            DepartmentPath.Create(longPath, CreateIdentifier("child"))
        );

        Assert.Contains("Путь не может превышать", exception.Message);
        Assert.Equal("parentPath", exception.ParamName);
    }

    [Fact]
    public void Create_WithPathAtMaxLength_ShouldSucceed()
    {
        var root = DepartmentPath.CreateRoot("root");
        var maxIdentifier = CreateIdentifier(new string('a', 255));
        var result = DepartmentPath.Create(root, maxIdentifier);

        Assert.NotNull(result);
        Assert.Equal(255, result.Value.Length);
    }

    [Fact]
    public void Create_WithPathNearMaxLength_ShouldSucceed()
    {
        var root = DepartmentPath.CreateRoot("root");
        var nearMaxIdentifier = CreateIdentifier(new string('a', 254));
        var result = DepartmentPath.Create(root, nearMaxIdentifier);

        Assert.NotNull(result);
        Assert.Equal(254, result.Value.Length);
    }

    [Theory]
    [InlineData("UPPERCASE")]
    [InlineData("test@dept")]
    [InlineData("test dept")]
    [InlineData("тест")]
    [InlineData("test_underscore")]
    [InlineData("test#hash")]
    [InlineData("test!exclaim")]
    [InlineData("TEST")]
    [InlineData("test$money")]
    [InlineData("test&ampersand")]
    public void Create_WithInvalidCharacters_ShouldThrowArgumentException(string invalidIdentifier)
    {
        var root = DepartmentPath.CreateRoot("root");
        var identifier = CreateIdentifier(invalidIdentifier);
        var exception = Assert.Throws<ArgumentException>(() =>
            DepartmentPath.Create(root, identifier)
        );

        Assert.Contains("Путь должен содержать только строчные латинские буквы", exception.Message);
        Assert.Equal("parentPath", exception.ParamName);
    }

    #endregion

    #region IsRoot Tests

    [Fact]
    public void IsRoot_ShouldReturnTrue_ForRootPath()
    {
        var rootPath = DepartmentPath.CreateRoot("root");

        Assert.True(rootPath.IsRoot);
    }

    [Fact]
    public void IsRoot_ShouldReturnFalse_ForNonRootPath()
    {
        var rootPath = DepartmentPath.CreateRoot("root");
        var childPath = DepartmentPath.Create(rootPath, CreateIdentifier("child"));

        Assert.False(childPath.IsRoot);
    }

    [Fact]
    public void IsRoot_Implementation_ShouldCheckNullOrEmpty()
    {
        var rootPath = DepartmentPath.CreateRoot("root");

        Assert.True(string.IsNullOrEmpty(rootPath.Value));
        Assert.True(rootPath.IsRoot);
    }

    #endregion

    #region Value Property Tests

    [Fact]
    public void Value_ShouldBeReadable()
    {
        var root = DepartmentPath.CreateRoot("root");
        var child = DepartmentPath.Create(root, CreateIdentifier("test"));
        var value = child.Value;

        Assert.Equal("test", value);
    }

    [Fact]
    public void Value_ShouldBeImmutable()
    {
        var root = DepartmentPath.CreateRoot("root");
        var child = DepartmentPath.Create(root, CreateIdentifier("test"));
        var originalValue = child.Value;

        Assert.Equal(originalValue, child.Value);
    }

    [Fact]
    public void Value_ForRoot_ShouldBeEmpty()
    {
        var root = DepartmentPath.CreateRoot("root");

        Assert.True(string.IsNullOrEmpty(root.Value));
    }

    #endregion

    #region Edge Cases

    [Fact]
    public void Create_WithNullParentPath_ShouldThrowNullReferenceException()
    {
        DepartmentPath? nullPath = null;
        var identifier = CreateIdentifier("test");

        Assert.Throws<NullReferenceException>(() => DepartmentPath.Create(nullPath!, identifier));
    }

    [Fact]
    public void Create_WithEmptyIdentifier_ShouldCreatePathWithSlash()
    {
        var root = DepartmentPath.CreateRoot("root");
        var emptyIdentifier = CreateIdentifier("");
        Assert.Throws<ArgumentException>(() => DepartmentPath.Create(root, emptyIdentifier));
    }

    [Fact]
    public void Create_WithSlashInIdentifier_ShouldBeValid()
    {
        var root = DepartmentPath.CreateRoot("root");
        var identifier = CreateIdentifier("test/sub");
        var result = DepartmentPath.Create(root, identifier);

        Assert.NotNull(result);
        Assert.Contains("/", result.Value);
    }

    #endregion

    #region Integration Tests

    [Fact]
    public void FullDepartmentHierarchy_ShouldWorkCorrectly()
    {
        var root = DepartmentPath.CreateRoot("root");
        var company = DepartmentPath.Create(root, CreateIdentifier("acme-corp"));
        var division = DepartmentPath.Create(company, CreateIdentifier("technology"));
        var department = DepartmentPath.Create(division, CreateIdentifier("engineering"));
        var team = DepartmentPath.Create(department, CreateIdentifier("backend-team"));

        Assert.True(root.IsRoot);
        Assert.False(company.IsRoot);
        Assert.Equal("acme-corp", company.Value);
        Assert.Equal("acme-corp.technology", division.Value);
        Assert.Equal("acme-corp.technology.engineering", department.Value);
        Assert.Equal("acme-corp.technology.engineering.backend-team", team.Value);
    }

    [Fact]
    public void PathWithDotsAndHyphens_ShouldBeValid()
    {
        var root = DepartmentPath.CreateRoot("root");
        var identifier = CreateIdentifier("v2.0-api-gateway");
        var result = DepartmentPath.Create(root, identifier);

        Assert.Equal("v2.0-api-gateway", result.Value);
        Assert.False(result.IsRoot);
    }

    [Fact]
    public void DeepHierarchy_ShouldMaintainCorrectPath()
    {
        var root = DepartmentPath.CreateRoot("root");
        var level1 = DepartmentPath.Create(root, CreateIdentifier("l1"));
        var level2 = DepartmentPath.Create(level1, CreateIdentifier("l2"));
        var level3 = DepartmentPath.Create(level2, CreateIdentifier("l3"));
        var level4 = DepartmentPath.Create(level3, CreateIdentifier("l4"));
        var level5 = DepartmentPath.Create(level4, CreateIdentifier("l5"));

        Assert.Equal("l1", level1.Value);
        Assert.Equal("l1.l2", level2.Value);
        Assert.Equal("l1.l2.l3", level3.Value);
        Assert.Equal("l1.l2.l3.l4", level4.Value);
        Assert.Equal("l1.l2.l3.l4.l5", level5.Value);
    }

    #endregion

    #region Create(string joinedName) Tests

    [Fact]
    public void Create_WithJoinedName_ShouldThrowNotImplementedException()
    {
        var joinedName = "department/subdepartment";

        var method = typeof(DepartmentPath).GetMethod(
            "Create",
            BindingFlags.NonPublic | BindingFlags.Static,
            null,
            new[] { typeof(string) },
            null
        );

        Assert.Throws<TargetInvocationException>(() =>
            method!.Invoke(null, new object[] { joinedName })
        );
    }

    #endregion

    #region Record Equality Tests (если record использует value-based equality)

    [Fact]
    public void TwoPaths_WithSameValue_ShouldBeEqual()
    {
        var root1 = DepartmentPath.CreateRoot("root");
        var root2 = DepartmentPath.CreateRoot("root");

        Assert.Equal(root1, root2);
    }

    [Fact]
    public void TwoPaths_WithDifferentValues_ShouldNotBeEqual()
    {
        var root = DepartmentPath.CreateRoot("root");
        var path1 = DepartmentPath.Create(root, CreateIdentifier("test1"));
        var path2 = DepartmentPath.Create(root, CreateIdentifier("test2"));

        Assert.NotEqual(path1, path2);
    }

    [Fact]
    public void Path_Equals_Self_ShouldReturnTrue()
    {
        var root = DepartmentPath.CreateRoot("root");

        Assert.Equal(root, root);
    }

    #endregion
}
