namespace Fereira.Costa.Domain.Infrastructure.Interfaces.Adapters;

public interface IDatabaseContextFactory
{
    Task<IDatabaseContext> CreateDbContextAsync();
    IDatabaseContext CreateDbContext();

}
