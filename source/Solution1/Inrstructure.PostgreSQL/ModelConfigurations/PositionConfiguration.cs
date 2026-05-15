using DirectoryService.Domain.PositionContext;
using DirectoryService.Domain.PositionContext.ValueObjects;
using DirectoryService.Domain.Shared.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.PostgreSQL.ModelConfigurations;

internal sealed class PositionConfiguration : IEntityTypeConfiguration<Position>
{
    public void Configure(EntityTypeBuilder<Position> builder)
    {
        builder.ToTable("positions");
        builder.HasKey(p => p.Id).HasName("pk_positions");

        builder
            .Property(p => p.Id)
            .HasConversion(id => id.Value, value => PositionId.Create(value))
            .HasColumnName("id")
            .IsRequired();

        builder
            .Property(n => n.Name)
            .HasConversion(n => n.Value, value => PositionName.Create(value))
            .HasColumnName("position_name")
            .HasMaxLength(255)
            .IsRequired();

        builder.HasIndex(n => n.Name).IsUnique().HasDatabaseName("ix_positions_name");

        builder
            .Property(d => d.Description)
            .HasConversion(d => d.Value, value => PositionDescription.Create(value))
            .HasColumnName("description")
            .HasColumnType("text")
            .IsRequired();

        builder
            .Property(p => p.IsActive)
            .HasConversion(isActive => isActive.Value, value => IsActive.Create(value))
            .HasColumnName("is_active")
            .IsRequired();

        builder.ComplexProperty(
            p => p.LifeTime,
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
            .HasMany<PosInDep>()
            .WithOne()
            .HasForeignKey(dp => dp.PositionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
