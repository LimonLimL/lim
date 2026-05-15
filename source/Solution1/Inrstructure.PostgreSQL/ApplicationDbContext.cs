using System;
using System.Collections.Generic;
using System.Text;
using DirectoryService.Domain.DepartmentContexts;
using DirectoryService.Domain.LocationsContext;
using DirectoryService.Domain.PositionContext;
using DirectoryService.Domain.Shared.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Inrstructure.PostgreSQL
{
    public class ApplicationDbContext : DbContext
    {
        private readonly PostgresSettings _settings;

        public ApplicationDbContext()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            _settings = new PostgresSettings();
            configuration.GetSection("PostgresSettings").Bind(_settings);
        }

        public ApplicationDbContext(IOptions<PostgresSettings> options)
        {
            _settings = options.Value;
        }

        public DbSet<Position> Positions => Set<Position>();
        public DbSet<Location> Locations => Set<Location>();
        public DbSet<Department> Departments => Set<Department>();
        public DbSet<PosInDep> DepartmentPositions => Set<PosInDep>();
        public DbSet<LocInDep> DepartmentLocations => Set<LocInDep>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_settings.ToConnectionString()).LogTo(Console.WriteLine);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }
    }
}
