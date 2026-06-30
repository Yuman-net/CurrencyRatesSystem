using UserService.Domain;

namespace UserService.Application.Abstractions.Repositories
{
    public interface IUserRepository
    {
        public Task<Guid> AddAsync(User user, CancellationToken cancellationToken);

        // default?
        public Task<User?> GetUserByName(string name, CancellationToken cancellationToken);

        public Task<bool> IsExistsAsync(string name, CancellationToken cancellationToken);
    }
}
