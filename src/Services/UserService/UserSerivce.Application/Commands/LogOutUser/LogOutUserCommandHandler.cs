using MediatR;
using UserService.Application.Abstractions.Repositories;

namespace UserService.Application.Commands.LogoutUser
{
    public sealed class LogoutUserCommandHandler : IRequestHandler<LogoutUserCommand>
    {
        private IRevorkedTokenRepository _revorkedTokenRepository;

        public LogoutUserCommandHandler(IRevorkedTokenRepository revorkedTokenRepository)
        {
            _revorkedTokenRepository = revorkedTokenRepository;
        }

        public async Task Handle(LogoutUserCommand request, CancellationToken cancellationToken)
        {
            var isRevokedToken = await _revorkedTokenRepository.IsRevorked(request.TokenId, cancellationToken);

            if (isRevokedToken)
            {
                return;
            }

            await _revorkedTokenRepository.RevorkeTokenAsync(request.TokenId, request.ExpiresAtUtc, cancellationToken);
        }
    }
}
