using DirectoryService.Domain.LocationsContext;
using DirectoryService.Domain.LocationsContext.ValueObjects;
using DirectoryService.Domain.Shared.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.PostgreSQL.ModelConfigurations;

internal sealed class LocationConfiguration : IEntityTypeConfiguration<Location>
{
    public void Configure(EntityTypeBuilder<Location> builder)
    {
        builder.ToTable("locations");
        builder.HasKey(l => l.Id).HasName("pk_locations");

        builder
            .Property(l => l.Id)
            .HasConversion(id => id.Value, value => LocationId.Create(value))
            .HasColumnName("id")
            .IsRequired();
        builder
            .Property(n => n.Name)
            .HasConversion(n => n.Value, value => LocationName.Create(value))
            .HasColumnName("location_name")
            .HasMaxLength(255)
            .IsRequired();

        builder.HasIndex(n => n.Name).IsUnique().HasDatabaseName("ix_locations_location_name");

        builder.ComplexProperty(
            l => l.Address,
            addressBuilder =>
            {
                addressBuilder
                    .Property(a => a.Value)
                    .HasColumnName("location_address")
                    .HasColumnType("text")
                    .IsRequired();
            }
        );
        builder
            .Property(t => t.TimeZone)
            .HasConversion(
                t => t.TimeZone,
                value => DirectoryService.Domain.Shared.ValueObjects.IanaTimeZone.Create(value)
            )
            .HasColumnName("iana_time_zone")
            .HasMaxLength(255)
            .IsRequired();

        builder.ComplexProperty(
            l => l.LifeTime,
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
            .HasMany<LocInDep>()
            .WithOne()
            .HasForeignKey(dl => dl.LocationId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
