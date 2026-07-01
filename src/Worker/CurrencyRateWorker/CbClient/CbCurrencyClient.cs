using CurrencyRateWorker.Dtos;
using System.Globalization;
using System.Xml.Linq;

namespace CurrencyRateWorker.CbClient
{
    public sealed class CbCurrencyClient
    {
        private const string CbUrl = "https://www.cbr.ru/scripts/XML_daily.asp";

        private const string Valute = nameof(Valute);
        private const string CharCode = nameof(CharCode);
        private const string Nominal = nameof(Nominal);
        private const string VunitRate = nameof(VunitRate);

        private static readonly CultureInfo CultureInfo = new ("ru-RU");

        private readonly HttpClient _httpClient;

        private readonly ILogger<CbCurrencyClient> _logger;

        public CbCurrencyClient(HttpClient httpClient, ILogger<CbCurrencyClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<IReadOnlyCollection<CbCurrencyRateDto>> GetCurrencyRateAsync(CancellationToken cancellationToken)
        {
            using var response = await _httpClient.GetAsync(CbUrl);

            response.EnsureSuccessStatusCode();

            var xml = await response.Content.ReadAsStringAsync(cancellationToken);

            var document = XDocument.Parse(xml);

            var result = document
                .Descendants(Valute)
                .Select(ParseCbCurrency)
                .Where(x => x is not null)
                .ToList();

            return result;
        }

        private static CbCurrencyRateDto? ParseCbCurrency(XElement element)
        {
            var charCode = element.Element(CharCode)?.Value;

            var nominal = element.Element(Nominal)?.Value;

            var vunitRateValue = element.Element(VunitRate)?.Value;

            if (string.IsNullOrWhiteSpace(charCode))
            {
                return null;
            }

            if (!string.IsNullOrWhiteSpace(vunitRateValue) &&
                decimal.TryParse(
                    vunitRateValue,
                    NumberStyles.Number,
                    CultureInfo,
                    out var vunitRate))
            {
                return new CbCurrencyRateDto(
                    charCode,
                    vunitRate);
            }

            return null;
        }
    }
}
