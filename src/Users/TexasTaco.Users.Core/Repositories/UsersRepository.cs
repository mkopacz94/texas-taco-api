﻿using Microsoft.EntityFrameworkCore;
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
                .FirstOrDefaultAsync(u => u.AccountId == accountId);
        }

        public async Task<User?> GetByIdAsync(UserId id)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task UpdateUserAsync(User user)
        {
            _context.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}
