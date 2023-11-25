using Microsoft.EntityFrameworkCore;
using TexasTaco.Authentication.Core.Models;

namespace TexasTaco.Authentication.Core.Data.EF
{
    internal class AuthDbContext(DbContextOptions<AuthDbContext> options) : DbContext(options)
    {
        public DbSet<Account> Accounts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }
}
