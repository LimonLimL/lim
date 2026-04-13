using DirectoryService.Domain.DepartmentContext.ValueObjects;
using DirectoryService.Domain.DepartmentContexts;
using DirectoryService.Domain.PositionContext;
using Xunit;

namespace TestProjectlim.PositionContextsTests
{
    public class RankTests
    {
        [Fact]
        public void Move_From_Low_Position_To_Higher()
        {
            Department department = CreateSampleDepartment(name: "it");

            Position position = CreateSamplePosition(name: "Директор");
            Position position2 = CreateSamplePosition(name: "Заместитель");
            Position position3 = CreateSamplePosition(name: "Старший сотрудник");
            Position position4 = CreateSamplePosition(name: "Сотрудник");
            Position position5 = CreateSamplePosition(name: "Младший сотрудник");

            PositionAdvertisement first = department.PublishAdvertisementForPosition(position);
            PositionAdvertisement second = department.PublishAdvertisementForPosition(position2);
            PositionAdvertisement third = department.PublishAdvertisementForPosition(position3);
            PositionAdvertisement fourth = department.PublishAdvertisementForPosition(position4);
            PositionAdvertisement fifth = department.PublishAdvertisementForPosition(position5);

            Assert.Equal("Директор", first.Position?.Name.Value);
            Assert.Equal("Заместитель", second.Position?.Name.Value);
            Assert.Equal("Старший сотрудник", third.Position?.Name.Value);
            Assert.Equal("Сотрудник", fourth.Position?.Name.Value);
            Assert.Equal("Младший сотрудник", fifth.Position?.Name.Value);

            department.MovePosition(second.PositionId, fourth.PositionId);

            Assert.Equal("Заместитель", second.Position?.Name.Value);
            Assert.Equal("Сотрудник", fourth.Position?.Name.Value);
        }

        private Department CreateSampleDepartment(string name)
        {
            return Department.CreateRoot(new DepartmentName(name));
        }

        private Position CreateSamplePosition(string name)
        {
            return new Position(name);
        }
    }
}
