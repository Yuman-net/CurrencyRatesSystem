using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UserService.Infrastructure.DataAccess;

namespace UserService.Infrastructure.Extensions
{
    public static class DataContextExtension
    {
        const string connectionString = "UserDBConnectionString";

        const string schema = "user";

        const string eFMigrationsHistory = "__EFMigrationsHistory";

        public static void AddUserDataContext(this IServiceCollection services, IConfiguration configuration)
        {
            var postgresConnectionString = configuration.GetConnectionString(connectionString);

            if (!string.IsNullOrWhiteSpace(postgresConnectionString))
            {
                services.AddDbContext<DataContext>(options =>
                    options.UseNpgsql(postgresConnectionString, opt => opt.MigrationsHistoryTable(eFMigrationsHistory, schema)));
            }
        }
    }
}
