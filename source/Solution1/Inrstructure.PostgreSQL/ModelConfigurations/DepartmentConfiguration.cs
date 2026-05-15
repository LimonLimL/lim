using DirectoryService.Domain.DepartmentContext.ValueObjects;
using DirectoryService.Domain.DepartmentContexts;
using DirectoryService.Domain.Shared.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.PostgreSQL.ModelConfigurations;

internal sealed class DepartmentConfiguration : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        builder.ToTable("departments");

        builder.HasKey(d => d.Id).HasName("pk_departments");
        builder
            .Property(d => d.Id)
            .HasConversion(id => id.Value, value => DepartmentId.From(value))
            .HasColumnName("id")
            .IsRequired();

        builder
            .Property(d => d.ParentId)
            .HasConversion(
                id => id != null ? id.Value : (Guid?)null,
                value => value != null ? DepartmentId.From(value) : null
            )
            .HasColumnName("parent_id")
            .IsRequired(false);

        builder
            .Property(d => d.Name)
            .HasConversion(name => name.Value, value => DepartmentName.Create(value))
            .HasColumnName("department_name")
            .HasMaxLength(255)
            .IsRequired();

        builder.HasIndex(d => d.Name).IsUnique().HasDatabaseName("ix_departments_department_name");

        builder
            .Property(d => d.Identifier)
            .HasConversion(id => id.Value, value => DepartmentIdentifier.Create(value))
            .HasColumnName("department_identifier")
            .HasMaxLength(255)
            .IsRequired();

        builder
            .HasIndex(d => d.Identifier)
            .IsUnique()
            .HasDatabaseName("ix_departments_department_identifier");

        builder
            .Property(d => d.Path)
            .HasConversion(path => path.Value, value => DepartmentPath.Create(value))
            .HasColumnName("department_path")
            .IsRequired();

        builder
            .Property(d => d.Depth)
            .HasConversion(depth => depth.Value, value => DepartmentDepth.Create((short)value))
            .HasColumnName("department_depth")
            .IsRequired();

        builder
            .Property(d => d.Level)
            .HasConversion(level => level.Value, value => HierarchyLevel.Create(value))
            .HasColumnName("hierarchy_level")
            .IsRequired();

        builder.ComplexProperty(
            d => d.LifeTime,
            lifeTimeBuilder =>
            {
                lifeTimeBuilder
                    .Property(lt => lt.CreatedAt)
                    .HasColumnName("created_at")
                    .IsRequired();
                lifeTimeBuilder
                    .Property(lt => lt.UpdatedAt)
                    .HasColumnName("updated_at")
                    .IsRequired(false);
                lifeTimeBuilder
                    .Property(lt => lt.DeletedAt)
                    .HasColumnName("deleted_at")
                    .IsRequired(false);
                lifeTimeBuilder
                    .Property(lt => lt.IsActivate)
                    .HasColumnName("is_active")
                    .IsRequired();
            }
        );

        builder
            .HasOne<Department>()
            .WithMany()
            .HasForeignKey(d => d.ParentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany<PosInDep>()
            .WithOne()
            .HasForeignKey(dp => dp.DepartmentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany<LocInDep>()
            .WithOne()
            .HasForeignKey(dl => dl.DepartmentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
