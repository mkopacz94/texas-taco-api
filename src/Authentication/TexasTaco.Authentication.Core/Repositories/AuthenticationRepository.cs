using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;
using TexasTaco.Authentication.Core.Data.EF;
using TexasTaco.Authentication.Core.Exceptions;
using TexasTaco.Authentication.Core.Models;
using TexasTaco.Authentication.Core.Services;
using TexasTaco.Authentication.Core.ValueObjects;

namespace TexasTaco.Authentication.Core.Repositories
{
    internal class AuthenticationRepository(
        IPasswordManager _passwordManager, 
        ISessionStorage _sessionStorage, 
        AuthDbContext _dbContext) : IAuthenticationRepository
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

        public async Task<SessionId> AuthenticateAccount(EmailAddress email, string password)
        {
            var account = await _dbContext.Accounts
                .FirstOrDefaultAsync(a => a.Email == email) 
                ?? throw new InvalidCredentialsException();

            if(!_passwordManager.VerifyPasswordHash(
                password, account.PasswordHash, account.PasswordSalt))
            {
                throw new InvalidCredentialsException();
            }

            return await _sessionStorage.CreateSession();
        }

        private Task<bool> EmailAlreadyExists(EmailAddress email)
        {
            return _dbContext.Accounts.AnyAsync(a => a.Email == email);
        }
    }
}
