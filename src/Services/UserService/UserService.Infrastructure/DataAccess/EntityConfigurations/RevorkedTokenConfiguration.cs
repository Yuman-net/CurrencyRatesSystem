using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.Domain;

namespace UserService.Infrastructure.DataAccess.EntityConfigurations
{
    public sealed class RevorkedTokenConfiguration : IEntityTypeConfiguration<RevokedToken>
    {
        public void Configure(EntityTypeBuilder<RevokedToken> builder)
        {
            builder.HasKey(x => x.TokenId);

            builder.Property(x => x.ExpiresAtUtc).IsRequired();

            builder.Property(x => x.RevokedAtUtc).IsRequired();

            builder.HasIndex(x => x.ExpiresAtUtc);
        }
    }
}
