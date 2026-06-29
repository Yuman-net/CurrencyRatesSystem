using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using UserService.Api.Authentication;
using UserService.Infrastructure.Authentification;

namespace UserService.Api.Extensions
{
    public static class JwtRegisterExtension
    {
        public static void AddJwtRegister(this IServiceCollection services, IConfiguration configuration)
        {
            // зачем тут Get?
            var jwtOptions = configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>()
                ?? throw new InvalidOperationException("Jwt options are not configured");

            if (jwtOptions.SecretKey is null)
            {
                throw new InvalidOperationException("Secret key are not configured");
            }

            services.AddScoped<RevokedTokenValidationEvents>();

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.EventsType = typeof(RevokedTokenValidationEvents);

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = jwtOptions.Issuer,
                        ValidateAudience = true,
                        ValidAudience = jwtOptions.Audience,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey)),
                        ClockSkew = TimeSpan.Zero
                    };
                });

            services.AddAuthorization();
        }
    }
}
