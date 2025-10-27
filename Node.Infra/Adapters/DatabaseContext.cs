
using Node.Domain.Entities;
using Node.Domain.Infrastructure.Interfaces.Adapters;
using Node.Domain.ValueObjects;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Node.Infra.Adapters;

public class DatabaseContext(
    DbContextOptions<DatabaseContext> options)
    : IdentityDbContext<User, UserRole, int>(options), IDataProtectionKeyContext, IDatabaseContext
{
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<DataProtectionKey> DataProtectionKeys { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        #region DbConfig
        modelBuilder.UseCollation("SQL_Latin1_General_CP1_CI_AI");

        modelBuilder.Entity<User>().OwnsOne(u => u.Address, a =>
        {
            a.Property(p => p.Street).HasMaxLength(1000);
            a.Property(p => p.Zipcode).HasMaxLength(20);
            a.Property(p => p.Number).HasMaxLength(20);
            a.Property(p => p.City).HasMaxLength(100);
            a.Property(p => p.Geolocation).HasMaxLength(50);
        });
        #endregion
    }
    public override async Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;

        var entityEntries = ChangeTracker.Entries<Entity>().ToList();
        foreach (var entry in entityEntries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = now;
            }
            if (entry.State is EntityState.Added or EntityState.Modified)
            {
                entry.Entity.UpdatedAt = now;
            }
        }

        var result = await base.SaveChangesAsync(cancellationToken);

        return result;
    }
}
