using UserService.Api.Extensions;
using UserService.Api.Middlewares;

namespace UserService.Api
{
    public class Program
    {
        private const string UserDBConnectionString = nameof(UserDBConnectionString);

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var configuration = builder.Configuration;

            builder.Services.AddServices(configuration);
            builder.Services.AddDataContext(configuration);
            builder.Services.AddJwtRegister(configuration);
            builder.Services.AddControllers();

            var app = builder.Build();

            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
