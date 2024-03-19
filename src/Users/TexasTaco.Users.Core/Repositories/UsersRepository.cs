using Microsoft.EntityFrameworkCore;
using TexasTaco.Users.Core.Data.EF;
using TexasTaco.Users.Core.Entities;
using TexasTaco.Users.Core.ValueObjects;

namespace TexasTaco.Users.Core.Repositories
{
    internal class UsersRepository(UsersDbContext _context) : IUsersRepository
    {
        public async Task AddUserAsync(User user)
        {
            await _context.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User?> GetByAccountIdAsync(Guid accountId)
        {
            return await _context.Users
                .Include(u => u.Address)
                .FirstOrDefaultAsync(u => u.AccountId == accountId);
        }

        public async Task UpdateUserAsync(User user)
        {
            _context.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}
