using MediatR;

namespace UserService.Application.Commands.LogInUser
{
    public sealed record LogInUserCommand(string Name, string Password) : IRequest<LoginUserCommandResult>;
}
