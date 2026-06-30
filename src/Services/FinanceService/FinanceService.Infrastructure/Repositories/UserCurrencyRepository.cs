using FinanceService.Application.Abstractions.Repositories;
using FinanceService.Application.QueryDtos;
using FinanceService.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace FinanceService.Infrastructure.Repositories
{
    public sealed class UserCurrencyRepository : IUserCurrencyRepository
    {
        private readonly DataContext _dataContext;

        public UserCurrencyRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<IReadOnlyCollection<FavoritesUserCurrencyDto>> GetFavoriesCurrencyByUserIdAsync(
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            var collection = await _dataContext.UserFavoriteCurrencies
                .AsNoTracking()
                .Where(x => x.UserId == userId)
                .Select(s => new FavoritesUserCurrencyDto(userId, s.Currency.Name, s.Currency.Rate))
                .ToListAsync(cancellationToken);

            return collection;
        }
    }
}
