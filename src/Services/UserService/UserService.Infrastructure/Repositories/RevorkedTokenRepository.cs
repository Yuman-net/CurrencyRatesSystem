using Microsoft.EntityFrameworkCore;
using UserService.Application.Abstractions.Repositories;
using UserService.Domain;
using UserService.Infrastructure.DataAccess;

namespace UserService.Infrastructure.Repositories
{
    public sealed class RevorkedTokenRepository : IRevorkedTokenRepository
    {
        private readonly DataContext _dataContext;

        public RevorkedTokenRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<bool> IsRevorked(string tokenId, CancellationToken cancellationToken)
        {
            return await _dataContext.RevokedTokens
                .AsNoTracking()
                .AnyAsync(x => x.TokenId == tokenId, cancellationToken);
        }

        public async Task RevorkeTokenAsync(string rokentId, DateTime expiresAtUtc, CancellationToken cancellationToken)
        {
            var revokeToken = RevokedToken.Create(rokentId, expiresAtUtc);

            await _dataContext.RevokedTokens.AddAsync(revokeToken);

            await _dataContext.SaveChangesAsync(cancellationToken);
        }
    }
}
