﻿using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TexasTaco.Shared.Pagination;
using TexasTaco.Shared.ValueObjects;
using TexasTaco.Users.Core.Data.EF;
using TexasTaco.Users.Core.Entities;
using TexasTaco.Users.Core.Exceptions;
using TexasTaco.Users.Core.ValueObjects;

namespace TexasTaco.Users.Core.Repositories
{
    internal class UsersRepository(UsersDbContext _context) : IUsersRepository
    {
        public async Task<IEnumerable<User>> GetUsers()
        {
            return await _context
                .Users
                .ToListAsync();
        }

        public async Task<PagedResult<User>> GetPagedUsersAsync(
            int pageNumber,
            int pageSize,
            Expression<Func<User, bool>>? filter)
        {
            IQueryable<User> query = _context.Users;

            if (filter is not null)
            {
                query = query.Where(u => EF.Functions.Like(u.Email.Value, $"%m.kop%"));
            }

            int totalCount = await query.CountAsync();
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new(items, totalCount, pageSize, pageNumber);
        }

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

        public async Task DeleteByAccountIdAsync(AccountId id)
        {
            var accountToDelete = await _context
                .Users
                .FirstOrDefaultAsync(a => a.AccountId == id.Value)
                ?? throw new UserNotFoundException(id);

            _context.Remove(accountToDelete);
            await _context.SaveChangesAsync();
        }
    }
}
