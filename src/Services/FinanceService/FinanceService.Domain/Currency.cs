namespace FinanceService.Domain
{
    public sealed class Currency
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public decimal Rate { get; set; }

        public ICollection<UserFavoriteCurrency> UserFavoriteCurrencies { get; set; } = new List<UserFavoriteCurrency>();

        private Currency(Guid id, string name, decimal rate)
        {
            Id = id;
            Name = name;
            Rate = rate;
        }

        public static Currency Create(string name, decimal rate)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Property name is required");
            }

            if (rate <= 0)
            {
                throw new ArgumentException("Currency rate must be greater than zero.", nameof(rate));
            }

            return new Currency(Guid.NewGuid(), name, rate);
        }

        public void UpdateRate(decimal rate)
        {
            {
                if (rate <= 0)
                    throw new ArgumentException("Currency rate must be greater than zero.", nameof(rate));

                Rate = rate;
            }
        }
    }
}
