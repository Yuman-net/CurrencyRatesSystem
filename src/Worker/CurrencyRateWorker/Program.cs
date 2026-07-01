using CurrencyRateWorker.CbClient;
using FinanceService.Application.Abstractions.Repositories;
using FinanceService.Infrastructure.Extensions;
using FinanceService.Infrastructure.Repositories;
using System.Text;

namespace CurrencyRateWorker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            builder.Services.AddServices(builder.Configuration);

            builder.Services.AddDataContext(builder.Configuration);

            builder.Services.AddHttpClient<CbCurrencyClient>();

            builder.Services.AddHostedService<Worker>();

            var host = builder.Build();

            host.Run();
        }
    }
}