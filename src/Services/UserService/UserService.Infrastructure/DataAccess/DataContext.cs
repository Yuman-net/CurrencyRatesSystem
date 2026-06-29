using Microsoft.EntityFrameworkCore;
using System.Reflection;
using UserService.Domain;

namespace UserService.Infrastructure.DataAccess
{
    public sealed class DataContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<RevokedToken> RevokedTokens { get; set; }

        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}