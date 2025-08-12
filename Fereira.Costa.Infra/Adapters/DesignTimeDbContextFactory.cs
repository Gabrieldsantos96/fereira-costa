using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Fereira.Costa.Infra.Adapters
{
    public class DesignTimeDbContextFactory: IDesignTimeDbContextFactory<DatabaseContext>
    {
        public DatabaseContext CreateDbContext(string[] args)
        {
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";

            var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile($"appsettings.{environmentName}.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

            string connectionString = configuration["DATABASE_URI"]!;

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