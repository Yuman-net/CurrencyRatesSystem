using UserService.Domain;

namespace UserService.Application.Abstractions.Services
{
    public interface IPasswordHasherService
    {
        public string HashPassword(string password);

        public bool VerifyPassword(string password, string hashPassword);
    }
}
