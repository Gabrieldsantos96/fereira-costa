using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Fereira.Costa.Infra.Adapters
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
    {
        public DatabaseContext CreateDbContext(string[] args)
        {
            string connectionString = Environment.GetEnvironmentVariable("DATABASE_URI")!;

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("A variável de ambiente DATABASE_URI não foi encontrada. Defina-a no ambiente ou no Azure.");
            }

            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseSqlServer(connectionString, builder =>
            {
                builder.EnableRetryOnFailure(3);
                builder.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
            });

            return new DatabaseContext(optionsBuilder.Options);
        }
    }
}