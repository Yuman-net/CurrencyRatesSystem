using FinanceService.Domain;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace FinanceService.Infrastructure.DataAccess
{
    public sealed class DataContext : DbContext
    {
        public DbSet<Currency> Currencies { get; set; }

        public DbSet<UserFavoriteCurrency> UserFavoriteCurrencies { get; set; }

        public DataContext(DbContextOptions<DataContext> options)
            : base (options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema("finance");

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
