using FinanceService.Infrastructure.Extensions;
using FinanseService.Api.Extensions;

namespace FinanseService.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var configuration = builder.Configuration;

            builder.Services.AddServices(configuration);
            builder.Services.AddDataContext(configuration);
            builder.Services.AddJwtRegister(configuration);
            builder.Services.AddControllers();

            var app = builder.Build();

            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
