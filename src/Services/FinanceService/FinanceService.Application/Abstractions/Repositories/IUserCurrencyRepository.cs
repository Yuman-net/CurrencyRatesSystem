using FinanceService.Application.QueryDtos;

namespace FinanceService.Application.Abstractions.Repositories
{
    public interface IUserCurrencyRepository
    {
        public Task<IReadOnlyCollection<FavoritesUserCurrencyDto>> GetFavoriesCurrencyByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    }
}
