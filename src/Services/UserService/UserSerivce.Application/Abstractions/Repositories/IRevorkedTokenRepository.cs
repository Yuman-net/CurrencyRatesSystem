namespace UserService.Application.Abstractions.Repositories
{
    public interface IRevorkedTokenRepository
    {
        public Task<bool> IsRevorked(string tokenId, CancellationToken cancellationToken);

        public Task RevorkeTokenAsync(string rokentId, DateTime expiresAtUtc, CancellationToken cancellationToken); 
    }
}
