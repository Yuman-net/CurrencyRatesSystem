using CurrencyRateWorker.CbClient;
using FinanceService.Domain;
using FinanceService.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace CurrencyRateWorker
{
    public class Worker : BackgroundService
    {
        private static readonly TimeSpan Interval = TimeSpan.FromHours(1);

        private readonly ILogger<Worker> _logger;

        private readonly IServiceScopeFactory _scopeFactory;

        public Worker(ILogger<Worker> logger, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Currency rate worker started.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await UpdateCurrencyRatesAsync(stoppingToken);
                }
                catch (Exception exception)
                {
                    _logger.LogError(
                    exception,
                    "Failed to update currency rates.");
                }

                await Task.Delay(Interval, stoppingToken);
            }
        }

        private async Task UpdateCurrencyRatesAsync(CancellationToken cancellationToken)
        {
            using var scope = _scopeFactory.CreateScope();

            var client = scope.ServiceProvider.GetRequiredService<CbCurrencyClient>();
            var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();

            var cbResults = await client.GetCurrencyRateAsync(cancellationToken);

            var currenciesFormDB = await dbContext.Currencies.ToListAsync(cancellationToken);

            foreach (var item in cbResults)
            {
                var isExistCurrency = currenciesFormDB.FirstOrDefault(x => x.Name == item.ChatCode);

                if (isExistCurrency is null)
                {
                    dbContext.Add(Currency.Create(item.ChatCode, item.Rate));

                    continue;
                }

                isExistCurrency.UpdateRate(item.Rate);
            }

            await dbContext.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Currency rates updated. Count: {Count}.", cbResults.Count);
        }
    }
}
