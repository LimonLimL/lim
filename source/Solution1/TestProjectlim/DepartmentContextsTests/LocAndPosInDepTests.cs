using DirectoryService.Domain.DepartmentContexts;
using DirectoryService.Domain.LocationsContext.ValueObjects;
using DirectoryService.Domain.PositionContext.ValueObjects;
using DirectoryService.Domain.Shared.ValueObjects;

namespace DirectoryService.Tests.DepartmentContexts
{
    public class LocAndPosInDepTests
    {
        [Fact]
        public void AddLoc_DuplicateLocationId_ThrowsInvalidOperationException()
        {
            var department = new Department();
            var locationId = LocationId.Create();
            var locInDep1 = new LocInDep(locationId);
            var locInDep2 = new LocInDep(locationId);

            department.AddLoc(locInDep1);

            var exception = Assert.Throws<InvalidOperationException>(() =>
                department.AddLoc(locInDep2)
            );
            Assert.Equal("ID не может повторяться внутри подразделения.", exception.Message);
        }

        [Fact]
        public void AddPos_DuplicatePositionId_ThrowsInvalidOperationException()
        {
            var department = new Department();
            var positionId = PositionId.Create();
            var posInDep1 = new PosInDep(positionId);
            var posInDep2 = new PosInDep(positionId);

            department.AddPos(posInDep1);
            var exception = Assert.Throws<InvalidOperationException>(() =>
                department.AddPos(posInDep2)
            );
            Assert.Equal("Должность не может повторяться внутри подразделения.", exception.Message);
        }
    }
}
