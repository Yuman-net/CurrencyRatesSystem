using Microsoft.EntityFrameworkCore;
using UserService.Application.Abstractions.Repositories;
using UserService.Domain;
using UserService.Infrastructure.DataAccess;

namespace UserService.Infrastructure.Repositories
{
    public sealed class UserRepository : IUserRepository
    {
        private readonly DataContext _dataContext;

        public UserRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<Guid> AddAsync(User user, CancellationToken cancellationToken)
        {
            await _dataContext.AddAsync(user, cancellationToken);

            await _dataContext.SaveChangesAsync(cancellationToken);

            return user.Id;
        }

        public async Task<User?> GetUserByName(string name, CancellationToken cancellationToken)
        {
            var user = await _dataContext.Users
                .AsNoTracking()
                .SingleOrDefaultAsync(u => u.Name == name, cancellationToken);

            return user;
        }

        public async Task<bool> IsExistsAsync(string name, CancellationToken cancellationToken)
        {
            var isExist = await _dataContext.Users
                .AnyAsync(x => x.Name == name, cancellationToken);

            return isExist;
        }
    }
}
