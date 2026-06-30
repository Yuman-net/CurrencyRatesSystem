using FinanceService.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace FinanseService.Api.Extensions
{
    public static class DataContextExtension
    {
        const string connectionString = "FinanceDBConnectionString";

        public static void AddDataContext(this IServiceCollection services, IConfiguration configuration)
        {
            var postgresConnectionString = configuration.GetConnectionString(connectionString);

            if (!string.IsNullOrWhiteSpace(postgresConnectionString))
            {
                services.AddDbContext<DataContext>(options => options.UseNpgsql(postgresConnectionString));
            }
        }
    }
}
