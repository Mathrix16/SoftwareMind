using Microsoft.EntityFrameworkCore;
using SoftwareMind.Dal.Entities;

namespace SoftwareMind.Dal;

public sealed class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Desk> Desks { get; set; } = null;
    public DbSet<Location> Locations { get; set; } = null;
    public DbSet<Reservation> Reservations { get; set; } = null;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Location>().HasData(new Location
            {
                Id = new Guid("7fd98d9a-20d9-4d4d-a576-bd42013139c6"),
                Name = "E2.3"
            },
            new Location
            {
                Id = new Guid("9b34d7d0-1413-40ff-8645-685c78b22265"),
                Name = "E2.8"
            });

        modelBuilder.Entity<Desk>().HasData(new Desk
            {
                Id = new Guid("3990fff7-324a-4579-b347-2e7b62845555"),
                LocationId = new Guid("7fd98d9a-20d9-4d4d-a576-bd42013139c6"),
                Name = "Przy Oknie",
                IsAvailable = true
            },
            new Desk
            {
                Id = new Guid("471904ac-1f95-418d-a6ce-81c7f8170a67"),
                LocationId = new Guid("9b34d7d0-1413-40ff-8645-685c78b22265"),
                Name = "Środek",
                IsAvailable = true
            });

        modelBuilder.Entity<Location>().HasKey(l => l.Id);
        modelBuilder.Entity<Desk>().HasKey(d => d.Id);
        modelBuilder.Entity<Reservation>().HasKey(r => r.Id);
    }
}