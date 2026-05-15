using DirectoryService.Domain.DepartmentContext;
using DirectoryService.Domain.DepartmentContexts;
using DirectoryService.Domain.PositionContext;
using DirectoryService.Domain.Shared.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.PostgreSQL.ModelConfigurations;

internal sealed class DepartmentPositionConfiguration : IEntityTypeConfiguration<PosInDep>
{
    public void Configure(EntityTypeBuilder<PosInDep> builder)
    {
        builder.ToTable("department_positions");
        builder
            .HasKey(dp => new { dp.PositionId, dp.DepartmentId })
            .HasName("pk_department_positions");
        builder.Property(dp => dp.PositionId).HasColumnName("id").IsRequired();
        builder.Property(dp => dp.DepartmentId).HasColumnName("department_id").IsRequired();
        builder
            .HasOne<Position>()
            .WithMany()
            .HasForeignKey(dp => dp.PositionId)
            .OnDelete(DeleteBehavior.Cascade);
        builder
            .HasOne<Department>()
            .WithMany()
            .HasForeignKey(dp => dp.DepartmentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
