using Core.Domain.Entities;
using Infrastructure.Seeders.UMS.Authentication;
using Infrastructure.Seeders.UMS.Profile;
using Microsoft.EntityFrameworkCore;
using UMS.Authentication.Domain.Entities;
using Microsoft.Extensions.Configuration;
using UMS.Profile.Domain.Entities;

namespace Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        if (string.IsNullOrEmpty(environment))
            environment = "Development";
        var connectionString = "";
        switch (environment)
        {
            case "Development":
                var configuration = new ConfigurationBuilder().AddJsonFile("dbsettings.json").Build();
                connectionString = configuration.GetSection("ConnectionStrings")["default"];
                break;
            case "Production":
                connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");
                break;
        }

        optionsBuilder.UseNpgsql(connectionString);
        optionsBuilder.UseLazyLoadingProxies();
    }

    // UMS.Authentication
    public DbSet<User>? Users { get; set; }
    public DbSet<UserChannel>? UserChannels { get; set; }
    public DbSet<Verification>? Verifications { get; set; }
    public DbSet<Login>? Logins { get; set; }
    public DbSet<PasswordReset>? PasswordResets { get; set; }
    public DbSet<Channel>? Channels { get; set; }

    // UMS.Profile
    public DbSet<Profile>? Profiles { get; set; }
    public DbSet<Address>? Addresses { get; set; }
    public DbSet<Country>? Countries { get; set; }
    public DbSet<Province>? Provinces { get; set; }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        foreach (var entry in ChangeTracker.Entries())
        {
            if (entry.Entity is BaseEntity entity)
            {
                switch (entry)
                {
                    case { State: EntityState.Added }:
                        entity.CreatedAt = DateTime.UtcNow;
                        entity.UpdatedAt = DateTime.UtcNow;
                        break;
                    case { State: EntityState.Modified }:
                        entity.UpdatedAt = DateTime.UtcNow;
                        break;
                }
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // UMS.Authentication
        new ChannelSeeder(modelBuilder).Seed();

        // UMS.Profile
        // Countries
        new CountrySeeder(modelBuilder).Seed();
        // Provinces
        var iran = Countries?.FirstOrDefault(c => c != null && c.Key == "IR", null);
        if (iran != null)
            new IranProvincesSeeder(modelBuilder, iran).Seed();

        base.OnModelCreating(modelBuilder);
    }
}