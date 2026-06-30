using FinanceService.Application.QueryDtos;

namespace FinanceService.Application.Queries
{
    public record CurrencyByUserResult(IReadOnlyCollection<FavoritesUserCurrencyDto> Favorites);
}
