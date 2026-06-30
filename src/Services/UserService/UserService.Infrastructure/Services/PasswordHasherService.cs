using Microsoft.AspNetCore.Identity;
using UserService.Application.Abstractions.Services;

namespace UserService.Infrastructure.Services
{
    public sealed class PasswordHasherService : IPasswordHasherService
    {
        private static readonly object PasswordHasherUser = new();

        private readonly IPasswordHasher<object> _passwordHasher;

        public PasswordHasherService(IPasswordHasher<object> passwordHasher)
        {
            _passwordHasher = passwordHasher;
        }

        public string HashPassword(string password)
        {
            // индексы?

            // if password == null
            var result = _passwordHasher.HashPassword(PasswordHasherUser, password);

            return result;
        }

        public bool VerifyPassword(string password, string hashPassword)
        {
            var result = _passwordHasher.VerifyHashedPassword(PasswordHasherUser, hashPassword, password);

            return result is PasswordVerificationResult.Success or PasswordVerificationResult.SuccessRehashNeeded;
        }
    }
}
