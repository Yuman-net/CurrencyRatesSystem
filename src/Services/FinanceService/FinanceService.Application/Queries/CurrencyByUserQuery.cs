using MediatR;

namespace FinanceService.Application.Queries
{
    public record CurrencyByUserQuery(Guid UserId) : IRequest<CurrencyByUserResult>;
}
