using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using UserSerivce.Application.Commands.RegisterUser;
using UserService.Application.Abstractions.Repositories;
using UserService.Application.Abstractions.Services;
using UserService.Infrastructure;
using UserService.Infrastructure.Authentification;
using UserService.Infrastructure.Repositories;

namespace UserService.Api.Extensions
{
    public static class ServicesExtensions
    {
        public static void AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IPasswordHasher<object>, PasswordHasher<object>>();
            services.AddScoped<IPasswordHasherService, PasswordHasherService>();
            
            services.AddScoped<IRevorkedTokenRepository, RevorkedTokenRepository>();

            services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(RegisterUserCommand).Assembly));

            
        }
    }
}
