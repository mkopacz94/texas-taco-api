using Microsoft.EntityFrameworkCore;
using TexasTaco.Authentication.Core.Entities;

namespace TexasTaco.Authentication.Core.Data.EF
{
    internal class AuthDbContext(DbContextOptions<AuthDbContext> options) : DbContext(options)
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<EmailNotification> EmailNotifications { get; set; }
        public DbSet<UserCreatedOutbox> UsersCreatedOutbox { get; set; }
        public DbSet<VerificationToken> VerificationTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }
}
