using Infrastructure.Seeders;
using Microsoft.EntityFrameworkCore;
using UMS.Authentication.Domain.Entities;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var configuration = new ConfigurationBuilder().AddJsonFile("dbsettings.json").Build();
        var connectionString = configuration.GetSection("ConnectionStrings")["Default"];
        optionsBuilder.UseNpgsql(connectionString);
        Console.Write($"Connection String: {connectionString}");
        optionsBuilder.UseLazyLoadingProxies();
    }

    public DbSet<User>? Users { get; set; }
    public DbSet<UserChannel>? UserChannels { get; set; }
    public DbSet<Verification>? Verifications { get; set; }
    public DbSet<Login>? Logins { get; set; }
    public DbSet<PasswordReset>? PasswordResets { get; set; }
    public DbSet<Channel>? Channels { get; set; }

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
        
        base.OnModelCreating(modelBuilder);
    }
}