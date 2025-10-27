using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Node.Domain.Entities;

namespace Node.Domain.Infrastructure.Interfaces.Adapters;
public interface IDatabaseContext : IDisposable, IAsyncDisposable
{
    ChangeTracker ChangeTracker { get; }

    public DbSet<User> Users { get; set; }

    public DbSet<RefreshToken> RefreshTokens { get; set; }

    DbSet<T> Set<T>() where T : class;

    EntityEntry<TEntity> Update<TEntity>(TEntity entity)
        where TEntity : class;
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    DatabaseFacade Database { get; }
    EntityEntry<TEntity> Entry<TEntity>(TEntity entity)
        where TEntity : class;

}
