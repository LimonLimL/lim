using DirectoryService.Domain.LocationsContext.ValueObjects;
using Xunit;

namespace DirectoryService.Domain.Tests.DepartmentContext.ValueObjects
{
    public class LocationIdTests
    {
        [Fact]
        public void Create_GeneratesNewGuid_Success()
        {
            var result = LocationId.Create();

            Assert.NotNull(result);
            Assert.NotEqual(Guid.Empty, result.Value);
        }

        [Fact]
        public void Create_DifferentCalls_ReturnsDifferentIds()
        {
            var id1 = LocationId.Create();
            var id2 = LocationId.Create();

            Assert.NotEqual(id1.Value, id2.Value);
        }

        [Fact]
        public void From_ValidGuid_Success()
        {
            var guid = Guid.NewGuid();

            var result = LocationId.From(guid);

            Assert.NotNull(result);
            Assert.Equal(guid, result.Value);
        }

        [Fact]
        public void From_EmptyGuid_ThrowsArgumentException()
        {
            var exception = Assert.Throws<ArgumentException>(() => LocationId.From(Guid.Empty));
            Assert.Equal("value", exception.ParamName);
            Assert.Contains("ID локации не может быть пустым", exception.Message);
        }

        [Fact]
        public void Constructor_EmptyGuid_ThrowsArgumentException()
        {
            var exception = Assert.Throws<ArgumentException>(() => LocationId.From(Guid.Empty));
            Assert.Equal("value", exception.ParamName);
        }

        [Fact]
        public void ToString_ReturnsGuidString()
        {
            var guid = Guid.NewGuid();
            var departmentId = LocationId.From(guid);

            var result = departmentId.ToString();

            Assert.Equal(guid.ToString(), result);
        }

        [Fact]
        public void Value_PropertyIsReadOnly()
        {
            var guid = Guid.NewGuid();
            var departmentId = LocationId.From(guid);

            Assert.Equal(guid, departmentId.Value);
        }
    }
}
