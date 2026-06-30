namespace FinanceService.Domain
{
    public sealed class UserFavoriteCurrency
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public Guid CurrencyId { get; set; }

        public Currency Currency { get; set; }

        public UserFavoriteCurrency Create(Guid userId, Guid currencyId)
        {
            return new UserFavoriteCurrency
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                CurrencyId = currencyId
            };
        }
    }
}
