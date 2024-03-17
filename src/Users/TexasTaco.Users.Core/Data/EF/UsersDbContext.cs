using Microsoft.EntityFrameworkCore;

namespace TexasTaco.Users.Core.Data.EF
{
    internal class UsersDbContext(DbContextOptions<UsersDbContext> options) : DbContext(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }
}
