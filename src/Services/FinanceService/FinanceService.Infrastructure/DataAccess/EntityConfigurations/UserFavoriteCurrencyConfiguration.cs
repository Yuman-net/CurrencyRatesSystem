using FinanceService.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinanceService.Infrastructure.DataAccess.EntityConfigurations
{
    public sealed class UserFavoriteCurrencyConfiguration : IEntityTypeConfiguration<UserFavoriteCurrency>
    {
        public void Configure(EntityTypeBuilder<UserFavoriteCurrency> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.UserId)
                .IsRequired();

            builder.Property(x => x.CurrencyId)
                .IsRequired();

            builder.HasOne(x => x.Currency)
                .WithMany(x => x.UserFavoriteCurrencies)
                .HasForeignKey(x => x.CurrencyId);

            builder.HasIndex(x => new { x.UserId, x.CurrencyId })
                .IsUnique();
        }
    }
}
