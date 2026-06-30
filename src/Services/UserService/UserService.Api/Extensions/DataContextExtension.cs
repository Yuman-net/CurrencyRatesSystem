using Microsoft.EntityFrameworkCore;
using UserService.Infrastructure.DataAccess;

namespace UserService.Api.Extensions
{
    public static class DataContextExtension
    {
        const string connectionString = "UserDBConnectionString";

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
