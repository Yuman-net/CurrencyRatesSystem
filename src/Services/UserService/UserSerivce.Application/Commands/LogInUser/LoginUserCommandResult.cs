namespace UserService.Application.Commands.LogInUser
{
    public sealed record class LoginUserCommandResult(Guid Id, string Name, string Token);
}
