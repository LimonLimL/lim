using DirectoryService.Domain.DepartmentContext.ValueObjects;
using Xunit;

namespace DirectoryService.Domain.Tests.DepartmentContext.ValueObjects;

public class DepartmentDepthTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(5)]
    [InlineData(10)]
    [InlineData(short.MaxValue)]
    public void Create_ValidDepth_Success(short depth)
    {
        var result = DepartmentDepth.Create(depth);

        Assert.NotNull(result);
        Assert.Equal(depth, result.Value);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-10)]
    [InlineData(short.MinValue)]
    public void Create_NegativeDepth_ThrowsArgumentException(short depth)
    {
        var exception = Assert.Throws<ArgumentException>(() => DepartmentDepth.Create(depth));
        Assert.Equal("depth", exception.ParamName);
        Assert.Contains("Глубина не может быть отрицательной", exception.Message);
    }

    [Fact]
    public void Root_ReturnsDepthZero()
    {
        var result = DepartmentDepth.Root;

        Assert.NotNull(result);
        Assert.Equal(0, result.Value);
    }

    [Theory]
    [InlineData(0, 1)]
    [InlineData(1, 2)]
    [InlineData(5, 6)]
    [InlineData(10, 11)]
    public void Increment_IncreasesDepthByOne(short initialValue, short expectedValue)
    {
        var depth = DepartmentDepth.Create(initialValue);

        var result = depth.Increment();

        Assert.NotNull(result);
        Assert.Equal(expectedValue, result.Value);
        Assert.Equal(initialValue, depth.Value);
    }
}
