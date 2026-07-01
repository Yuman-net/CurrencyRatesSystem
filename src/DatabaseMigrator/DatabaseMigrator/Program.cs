using FinanceService.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using UserService.Infrastructure.Extensions;

using UserDataConext = UserService.Infrastructure.DataAccess.DataContext;
using FinanceDatacontext = FinanceService.Infrastructure.DataAccess.DataContext;


namespace DatabaseMigrator
{
    internal class Program
    {
        private const string DbMigrator = nameof(DbMigrator);
        private const string UserDBConnectionString = nameof(UserDBConnectionString);

        private const string FinanceDBConnectionString = nameof(FinanceDBConnectionString);

        static async Task Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);

            var userDbContextString = builder.Configuration.GetConnectionString(UserDBConnectionString);

            var financeDbContextString = builder.Configuration.GetConnectionString(FinanceDBConnectionString);

            if (string.IsNullOrWhiteSpace(userDbContextString))
            {
                throw new InvalidOperationException($"Connection string '{UserDBConnectionString}' is not configured");
            }

            if (string.IsNullOrWhiteSpace(financeDbContextString))
            {
                throw new InvalidOperationException($"Connection string '{FinanceDBConnectionString}' is not configured");
            }

            builder.Services.AddUserDataContext(builder.Configuration);
            builder.Services.AddFinanceDataContext(builder.Configuration);

            var host = builder.Build();

            using var scope = host.Services.CreateScope();

            var logger = scope.ServiceProvider
                .GetRequiredService<ILoggerFactory>()
                .CreateLogger(DbMigrator);

            try
            {
                await MigrateDataBaseAsync<UserDataConext>(logger, scope);

                await MigrateDataBaseAsync<FinanceDatacontext>(logger, scope);
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "Database migration failed.");
                throw;
            }
        }

        private static async Task MigrateDataBaseAsync<TContext>(ILogger logger, IServiceScope scope)
            where TContext : DbContext
        {
            logger.LogInformation($"Applying {nameof(TContext)} migrations...");

            var userDbContext = scope.ServiceProvider.GetRequiredService<TContext>();

            await userDbContext.Database.MigrateAsync();

            logger.LogInformation($"{nameof(TContext)} migrations applied.");
        }
    }
}
