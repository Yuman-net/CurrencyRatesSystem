using MediatR;

namespace UserService.Application.Commands.LogoutUser
{
    public sealed record LogoutUserCommand(string TokenId, DateTime ExpiresAtUtc) : IRequest;
}
