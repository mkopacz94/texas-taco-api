using Microsoft.EntityFrameworkCore;
using TexasTaco.Users.Core.Entities;

namespace TexasTaco.Users.Core.Data.EF
{
    internal class UsersDbContext(DbContextOptions<UsersDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<AccountCreatedInboxMessage> AccountCreatedInboxMessages { get; set; }
        public DbSet<UserUpdatedOutboxMessage> UserUpdatedOutboxMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }
}
