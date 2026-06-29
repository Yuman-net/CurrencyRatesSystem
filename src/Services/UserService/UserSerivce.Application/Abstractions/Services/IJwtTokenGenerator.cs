using UserService.Domain;

namespace UserService.Application.Abstractions.Services
{
    public interface IJwtTokenGenerator
    {
        public string GenerateJwtToken(Guid userId, string userName);
    }
}
