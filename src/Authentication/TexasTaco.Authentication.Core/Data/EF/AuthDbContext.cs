using Microsoft.EntityFrameworkCore;
using TexasTaco.Authentication.Core.Entities;
using TexasTaco.Shared.EventBus.Account;
using TexasTaco.Shared.Outbox;

namespace TexasTaco.Authentication.Core.Data.EF
{
    internal class AuthDbContext(DbContextOptions<AuthDbContext> options) : DbContext(options)
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<EmailNotification> EmailNotifications { get; set; }
        public DbSet<AccountCreatedOutbox> AccountsCreatedOutbox { get; set; }
        public DbSet<OutboxMessage<AccountDeletedEventMessage>> AccountDeletedOutboxMessages { get; set; }
        public DbSet<VerificationToken> VerificationTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }
}
