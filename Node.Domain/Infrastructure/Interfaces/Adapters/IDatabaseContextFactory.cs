namespace Node.Domain.Infrastructure.Interfaces.Adapters;

public interface IDatabaseContextFactory
{
    Task<IDatabaseContext> CreateDbContextAsync();
    IDatabaseContext CreateDbContext();

}
