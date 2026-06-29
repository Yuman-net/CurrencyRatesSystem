namespace UserService.Api.Contracts.OutModels
{
    public sealed record LoginUserResponse(Guid Id, string Name, string AccessToken);
}
