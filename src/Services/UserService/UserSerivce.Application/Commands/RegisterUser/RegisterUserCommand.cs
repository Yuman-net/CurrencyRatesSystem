using MediatR;

namespace UserSerivce.Application.Commands.RegisterUser
{
    public sealed record class RegisterUserCommand(string Name, string Password) : IRequest<Guid>;
}