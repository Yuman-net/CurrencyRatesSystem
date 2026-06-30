using FinanceService.Application.Abstractions.Repositories;
using FinanceService.Application.Queries;
using FinanceService.Infrastructure.Repositories;

namespace FinanseService.Api.Extensions
{
    public static class ServicesExtensions
    {
        public static void AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IUserCurrencyRepository, UserCurrencyRepository>();

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CurrencyByUserQuery).Assembly));
        }
    }
}
