using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using UserService.Application.Abstractions.Repositories;

namespace UserService.Api.Authentication
{
    public sealed class RevokedTokenValidationEvents : JwtBearerEvents
    {
        private readonly IRevorkedTokenRepository _revorkedTokenRepository;

        public RevokedTokenValidationEvents(IRevorkedTokenRepository revorkedTokenRepository)
        {
            _revorkedTokenRepository = revorkedTokenRepository;
        }

        public override async Task TokenValidated(TokenValidatedContext context)
        {
            var jti = context.Principal?.FindFirstValue(JwtRegisteredClaimNames.Jti);

            if (jti is null)
            {
                context.Fail("Token is missing.");

                return;
            }

            var isRevoked = await _revorkedTokenRepository.IsRevorked(jti, context.HttpContext.RequestAborted);

            if (isRevoked)
            {
                context.Fail("Token has been revoked.");
            }
        }
    }
}
