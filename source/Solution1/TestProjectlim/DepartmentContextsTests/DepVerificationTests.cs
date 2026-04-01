using System;
using System.Collections.Generic;
using System.Reflection;
using DirectoryService.Domain.DepartmentContext.ValueObjects;
using DirectoryService.Domain.DepartmentContexts;
using Xunit;

namespace DirectoryService.Tests.DepartmentContext.ValueObjects
{
    public class DepVerificationTests
    {
        #region Constructor Tests

        [Fact]
        public void Constructor_WhenExistingDepartmentsIsNull_CreatesEmptyList()
        {
            var verification = new DepVerification(null!);
            var identifier = DepartmentIdentifier.Create("test-id");
            var result = verification.CheckIdentifierUniqueness(identifier);

            Assert.True(result);
        }

        [Fact]
        public void Constructor_WhenExistingDepartmentsProvided_StoresList()
        {
            var departments = new List<Department>
            {
                CreateDepartment(Guid.NewGuid(), "IT", "dep-1"),
                CreateDepartment(Guid.NewGuid(), "HR", "dep-2"),
            };

            var verification = new DepVerification(departments);
            var newDepartment = CreateDepartment(Guid.NewGuid(), "Finance", "dep-3");
            var isUnique = verification.CheckUniqueness(newDepartment);

            Assert.True(isUnique);
        }

        #endregion

        #region CheckUniqueness Tests

        [Fact]
        public void CheckUniqueness_WhenDepartmentIsNull_ReturnsFalse()
        {
            var verification = new DepVerification(new List<Department>());
            var result = verification.CheckUniqueness(null!);

            Assert.False(result);
        }

        [Fact]
        public void CheckUniqueness_WhenListIsEmpty_ReturnsTrue()
        {
            var verification = new DepVerification(new List<Department>());
            var department = CreateDepartment(Guid.NewGuid(), "IT", "dep-1");
            var result = verification.CheckUniqueness(department);

            Assert.True(result);
        }

        [Fact]
        public void CheckUniqueness_WhenDepartmentNameIsUnique_ReturnsTrue()
        {
            var existingDepartments = new List<Department>
            {
                CreateDepartment(Guid.NewGuid(), "IT", "dep-1"),
                CreateDepartment(Guid.NewGuid(), "HR", "dep-2"),
            };
            var verification = new DepVerification(existingDepartments);
            var newDepartment = CreateDepartment(Guid.NewGuid(), "Finance", "dep-3");
            var result = verification.CheckUniqueness(newDepartment);

            Assert.True(result);
        }

        [Fact]
        public void CheckUniqueness_WhenDepartmentNameExistsWithDifferentId_ReturnsFalse()
        {
            var existingDepartments = new List<Department>
            {
                CreateDepartment(Guid.NewGuid(), "IT", "dep-1"),
                CreateDepartment(Guid.NewGuid(), "HR", "dep-2"),
            };
            var verification = new DepVerification(existingDepartments);
            var duplicateDepartment = CreateDepartment(Guid.NewGuid(), "IT", "dep-3");
            var result = verification.CheckUniqueness(duplicateDepartment);

            Assert.False(result);
        }

        [Fact]
        public void CheckUniqueness_WhenDepartmentIsSame_ReturnsTrue()
        {
            var existingDepartment = CreateDepartment(Guid.NewGuid(), "IT", "dep-1");
            var existingDepartments = new List<Department> { existingDepartment };
            var verification = new DepVerification(existingDepartments);
            var result = verification.CheckUniqueness(existingDepartment);

            Assert.True(result);
        }

        #endregion

        #region CheckIdentifierUniqueness Tests

        [Fact]
        public void CheckIdentifierUniqueness_WhenListIsEmpty_ReturnsTrue()
        {
            var verification = new DepVerification(new List<Department>());
            var identifier = DepartmentIdentifier.Create("dep-1");
            var result = verification.CheckIdentifierUniqueness(identifier);

            Assert.True(result);
        }

        [Fact]
        public void CheckIdentifierUniqueness_WhenIdentifierIsUnique_ReturnsTrue()
        {
            var existingDepartments = new List<Department>
            {
                CreateDepartment(Guid.NewGuid(), "IT", "dep-1"),
                CreateDepartment(Guid.NewGuid(), "HR", "dep-2"),
            };
            var verification = new DepVerification(existingDepartments);
            var newIdentifier = DepartmentIdentifier.Create("dep-3");
            var result = verification.CheckIdentifierUniqueness(newIdentifier);

            Assert.True(result);
        }

        [Fact]
        public void CheckIdentifierUniqueness_WhenIdentifierExists_ReturnsFalse()
        {
            var existingDepartments = new List<Department>
            {
                CreateDepartment(Guid.NewGuid(), "IT", "dep-1"),
                CreateDepartment(Guid.NewGuid(), "HR", "dep-2"),
            };
            var verification = new DepVerification(existingDepartments);
            var duplicateIdentifier = DepartmentIdentifier.Create("dep-1");
            var result = verification.CheckIdentifierUniqueness(duplicateIdentifier);

            Assert.False(result);
        }

        [Fact]
        public void CheckIdentifierUniqueness_CaseSensitiveComparison()
        {
            var existingDepartments = new List<Department>
            {
                CreateDepartment(Guid.NewGuid(), "IT", "dep-1"),
            };
            var verification = new DepVerification(existingDepartments);
            var differentCaseIdentifier = DepartmentIdentifier.Create("DEP-1");
            var result = verification.CheckIdentifierUniqueness(differentCaseIdentifier);

            Assert.True(result);
        }

        #endregion

        #region Helper Methods

        private Department CreateDepartment(
            Guid id,
            string name,
            string identifier,
            Guid? parentId = null,
            int level = 1,
            bool isActive = true
        )
        {
            DepartmentId departmentId = DepartmentId.From(id);
            DepartmentName departmentName = DepartmentName.Create(name);
            DepartmentIdentifier departmentIdentifier = DepartmentIdentifier.Create(identifier);
            DepartmentPath departmentPath = DepartmentPath.СоздатьИзИдентификатора(
                departmentIdentifier
            );
            HierarchyLevel hierarchyLevel = HierarchyLevel.Create(level);
            DepartmentId? departmentdid = null;
            if (parentId != null)
            {
                departmentdid = DepartmentId.From(parentId.Value);
            }
            return new Department(
                departmentId,
                departmentName,
                departmentIdentifier,
                departmentdid,
                departmentPath,
                hierarchyLevel,
                isActive
            );
        }

        #endregion
    }
}
