namespace FinanceService.Application.QueryDtos
{
    public record FavoritesUserCurrencyDto(Guid userId, string CurrencyName, decimal Rate);
}
