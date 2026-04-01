using DirectoryService.Domain.DepartmentContext.ValueObjects;
using Xunit;

namespace DirectoryService.Domain.Tests.DepartmentContext.ValueObjects
{
    public class DepartmentIdTests
    {
        [Fact]
        public void Create_GeneratesNewGuid_Success()
        {
            var result = DepartmentId.Create();

            Assert.NotNull(result);
            Assert.NotEqual(Guid.Empty, result.Value);
        }

        [Fact]
        public void Create_DifferentCalls_ReturnsDifferentIds()
        {
            var id1 = DepartmentId.Create();
            var id2 = DepartmentId.Create();

            Assert.NotEqual(id1.Value, id2.Value);
        }

        [Fact]
        public void From_ValidGuid_Success()
        {
            var guid = Guid.NewGuid();

            var result = DepartmentId.From(guid);

            Assert.NotNull(result);
            Assert.Equal(guid, result.Value);
        }

        [Fact]
        public void From_EmptyGuid_ThrowsArgumentException()
        {
            var exception = Assert.Throws<ArgumentException>(() => DepartmentId.From(Guid.Empty));
            Assert.Equal("value", exception.ParamName);
            Assert.Contains("ID подразделения не может быть пустым", exception.Message);
        }

        [Fact]
        public void Constructor_EmptyGuid_ThrowsArgumentException()
        {
            var exception = Assert.Throws<ArgumentException>(() => DepartmentId.From(Guid.Empty));
            Assert.Equal("value", exception.ParamName);
        }

        [Fact]
        public void ToString_ReturnsGuidString()
        {
            var guid = Guid.NewGuid();
            var departmentId = DepartmentId.From(guid);

            var result = departmentId.ToString();

            Assert.Equal(guid.ToString(), result);
        }

        [Fact]
        public void Value_PropertyIsReadOnly()
        {
            var guid = Guid.NewGuid();
            var departmentId = DepartmentId.From(guid);

            Assert.Equal(guid, departmentId.Value);
        }
    }
}
