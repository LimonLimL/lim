using DirectoryService.Domain.DepartmentContext;
using DirectoryService.Domain.DepartmentContexts;
using DirectoryService.Domain.LocationsContext;
using DirectoryService.Domain.Shared.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.PostgreSQL.ModelConfigurations;

internal sealed class DepartmentLocationConfiguration : IEntityTypeConfiguration<LocInDep>
{
    public void Configure(EntityTypeBuilder<LocInDep> builder)
    {
        builder.ToTable("department_locations");
        builder
            .HasKey(dl => new { dl.LocationId, dl.DepartmentId })
            .HasName("pk_department_locations");
        builder.Property(dl => dl.LocationId).HasColumnName("id").IsRequired();
        builder.Property(dl => dl.DepartmentId).HasColumnName("department_id").IsRequired();
        builder
            .HasOne<Location>()
            .WithMany()
            .HasForeignKey(dl => dl.LocationId)
            .OnDelete(DeleteBehavior.Cascade);
        builder
            .HasOne<Department>()
            .WithMany()
            .HasForeignKey(dl => dl.DepartmentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
