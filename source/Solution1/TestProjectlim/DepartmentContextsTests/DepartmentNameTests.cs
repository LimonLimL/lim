using DirectoryService.Domain.DepartmentContext.ValueObjects;
using Xunit;

namespace DirectoryService.Domain.Tests.DepartmentContext.ValueObjects
{
    public class DepartmentNameTests
    {
        [Theory]
        [InlineData("Отдел разработки")]
        [InlineData("Бухгалтерия")]
        [InlineData("IT")]
        [InlineData("a")]
        [InlineData("Отдел поддержки пользователей с очень длинным названием")]
        public void Create_ValidName_Success(string name)
        {
            var result = DepartmentName.Create(name);

            Assert.NotNull(result);
            Assert.Equal(name, result.Value);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public void Create_NullOrWhitespace_ThrowsArgumentException(string name)
        {
            var exception = Assert.Throws<ArgumentException>(() => DepartmentName.Create(name));

            Assert.Equal("name", exception.ParamName);
            Assert.Contains("Имя подразделения не может быть пустым", exception.Message);
        }

        [Fact]
        public void Create_TrimWhitespace_Success()
        {
            var name = "  Отдел разработки  ";

            var result = DepartmentName.Create(name);

            Assert.NotNull(result);
            Assert.Equal("Отдел разработки", result.Value);
        }

        [Fact]
        public void Create_MaxLengthExactly_Success()
        {
            var maxLengthName = new string('a', 100);

            var result = DepartmentName.Create(maxLengthName);

            Assert.NotNull(result);
            Assert.Equal(maxLengthName, result.Value);
            Assert.Equal(100, result.Value.Length);
        }

        [Fact]
        public void Create_ExceedsMaxLength_ThrowsArgumentException()
        {
            var tooLongName = new string('a', 101);

            var exception = Assert.Throws<ArgumentException>(() =>
                DepartmentName.Create(tooLongName)
            );

            Assert.Equal("name", exception.ParamName);
            Assert.Contains("Имя подразделения не может превышать 100 символов", exception.Message);
        }

        [Fact]
        public void Create_TrimThenValidateMaxLength_Success()
        {
            var nameWithSpaces = "  " + new string('a', 100) + "  ";

            var result = DepartmentName.Create(nameWithSpaces);

            Assert.NotNull(result);
            Assert.Equal(100, result.Value.Length);
        }

        [Fact]
        public void Value_PropertyIsReadOnly()
        {
            var name = DepartmentName.Create("Отдел разработки");

            Assert.Equal("Отдел разработки", name.Value);
        }

        [Fact]
        public void ToString_ReturnsNameString()
        {
            var name = DepartmentName.Create("Бухгалтерия");

            var result = name.ToString();

            Assert.Equal("Бухгалтерия", result);
        }

        [Fact]
        public void Create_DifferentNames_ReturnsDifferentInstances()
        {
            var name1 = DepartmentName.Create("Отдел 1");
            var name2 = DepartmentName.Create("Отдел 2");

            Assert.NotEqual(name1, name2);
        }

        [Fact]
        public void Create_SameNames_ReturnsEqualInstances()
        {
            var name1 = DepartmentName.Create("Одинаковое имя");
            var name2 = DepartmentName.Create("Одинаковое имя");

            Assert.Equal(name1, name2);
        }

        [Theory]
        [InlineData("  test  ")]
        [InlineData("   отдел   ")]
        [InlineData(" name ")]
        public void Create_TrimsLeadingAndTrailingWhitespace_Success(string name)
        {
            var result = DepartmentName.Create(name);

            Assert.NotNull(result);
            Assert.Equal(name.Trim(), result.Value);
        }

        [Fact]
        public void Create_UnicodeCharacters_Success()
        {
            var name = "Отдел 🚀 разработки";

            var result = DepartmentName.Create(name);

            Assert.NotNull(result);
            Assert.Equal(name, result.Value);
        }

        [Fact]
        public void Create_SpecialCharacters_Success()
        {
            var name = "Отдел №1 (основной)";

            var result = DepartmentName.Create(name);

            Assert.NotNull(result);
            Assert.Equal(name, result.Value);
        }

        [Fact]
        public void Create_Length99_Success()
        {
            var name = new string('a', 99);

            var result = DepartmentName.Create(name);

            Assert.NotNull(result);
            Assert.Equal(99, result.Value.Length);
        }

        [Fact]
        public void Create_Length101_ThrowsArgumentException()
        {
            var name = new string('a', 101);

            var exception = Assert.Throws<ArgumentException>(() => DepartmentName.Create(name));

            Assert.Equal("name", exception.ParamName);
        }
    }
}
