using Microsoft.EntityFrameworkCore;
using TexasTaco.Shared.EventBus.Account;
using TexasTaco.Shared.EventBus.Orders;
using TexasTaco.Shared.EventBus.Users;
using TexasTaco.Shared.Inbox;
using TexasTaco.Shared.Outbox;
using TexasTaco.Users.Core.Entities;

namespace TexasTaco.Users.Core.Data.EF
{
    internal class UsersDbContext(DbContextOptions<UsersDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<InboxMessage<AccountCreatedEventMessage>> AccountCreatedInboxMessages { get; set; }
        public DbSet<OutboxMessage<UserUpdatedEventMessage>> UserUpdatedOutboxMessages { get; set; }
        public DbSet<InboxMessage<PointsCollectedEventMessage>> PointsCollectedInboxMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }
}
