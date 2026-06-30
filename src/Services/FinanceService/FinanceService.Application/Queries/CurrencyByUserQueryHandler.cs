using FinanceService.Application.Abstractions.Repositories;
using MediatR;

namespace FinanceService.Application.Queries
{
    public sealed class CurrencyByUserQueryHandler : IRequestHandler<CurrencyByUserQuery, CurrencyByUserResult>
    {
        private IUserCurrencyRepository _userCurrencyRepository;

        public CurrencyByUserQueryHandler(IUserCurrencyRepository userCurrencyRepository)
        {
            _userCurrencyRepository = userCurrencyRepository;
        }

        public async Task<CurrencyByUserResult> Handle(CurrencyByUserQuery request, CancellationToken cancellationToken)
        {
            var favoriteCurrencies = await _userCurrencyRepository.GetFavoriesCurrencyByUserIdAsync(request.UserId, cancellationToken);

            return new CurrencyByUserResult(favoriteCurrencies);
        }
    }
}
