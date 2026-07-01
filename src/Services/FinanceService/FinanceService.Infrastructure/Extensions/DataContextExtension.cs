using FinanceService.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FinanceService.Infrastructure.Extensions
{
    public static class DataContextExtension
    {
        const string connectionString = "FinanceDBConnectionString";

        const string eFMigrationsHistory = "__EFMigrationsHistory";

        const string financeSchema = "finance";

        public static void AddFinanceDataContext(this IServiceCollection services, IConfiguration configuration)
        {
            var postgresConnectionString = configuration.GetConnectionString(connectionString);

            if (!string.IsNullOrWhiteSpace(postgresConnectionString))
            {
                services.AddDbContext<DataContext>(options =>
                {
                    options.UseNpgsql(postgresConnectionString, opt => opt.MigrationsHistoryTable(eFMigrationsHistory, financeSchema));
                });
            }
        }
    }
}
