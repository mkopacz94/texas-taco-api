using Microsoft.EntityFrameworkCore;
using TexasTaco.Authentication.Core.Data.EF;
using TexasTaco.Authentication.Core.Exceptions;
using TexasTaco.Authentication.Core.Models;
using TexasTaco.Authentication.Core.Services;
using TexasTaco.Authentication.Core.ValueObjects;

namespace TexasTaco.Authentication.Core.Repositories
{
    internal class AuthenticationRepository(
        IPasswordManager _passwordManager, AuthDbContext _dbContext) : IAuthenticationRepository
    {
        public async Task CreateAccount(EmailAddress email, Role role, string password)
        {
            if (await EmailAlreadyExists(email))
            {
                throw new EmailAlreadyExistsException(email);
            }

            _passwordManager.HashPassword(password, out byte[] passwordHash, out byte[] passwordSalt);

            var account = new Account(email, role, passwordHash, passwordSalt);

            await _dbContext.Accounts.AddAsync(account);
            await _dbContext.SaveChangesAsync();
        }

        public Task<bool> EmailAlreadyExists(EmailAddress email)
        {
            return _dbContext.Accounts.AnyAsync(a => a.Email == email);
        }
    }
}
