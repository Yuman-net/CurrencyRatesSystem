namespace UserService.Domain
{
    public sealed class RevokedToken
    {
        public string TokenId { get; init; }

        public DateTime ExpiresAtUtc { get; init; }

        public DateTime RevokedAtUtc { get; init; }

        private RevokedToken(string tokenId, DateTime expiresAtUtc)
        {
            TokenId = tokenId;
            ExpiresAtUtc = expiresAtUtc;
            RevokedAtUtc = DateTime.UtcNow;
        }

        public static RevokedToken Create(string tokenId, DateTime expiresAtUtc)
        {
            if (string.IsNullOrWhiteSpace(tokenId))
            {
                throw new ArgumentException($"{nameof(tokenId)} is required");
            }

            return new RevokedToken(tokenId, expiresAtUtc);
        }
    }
}
