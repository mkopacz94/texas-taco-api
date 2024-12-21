using Microsoft.EntityFrameworkCore;
using TexasTaco.Authentication.Core.Data.EF;
using TexasTaco.Authentication.Core.Entities;
using TexasTaco.Authentication.Core.Exceptions;
using TexasTaco.Authentication.Core.Services;
using TexasTaco.Shared.Authentication;
using TexasTaco.Shared.ValueObjects;

namespace TexasTaco.Authentication.Core.Repositories
{
    internal class AuthenticationRepository(
        IPasswordManager _passwordManager,
        AuthDbContext _dbContext) : IAuthenticationRepository
    {
        public async Task<Account> CreateAccountAsync(EmailAddress email, Role role, string password)
        {
            if (await EmailAlreadyExists(email))
            {
                throw new EmailAlreadyExistsException(email);
            }

            _passwordManager.HashPassword(password, out byte[] passwordHash, out byte[] passwordSalt);

            var account = new Account(email, role, passwordHash, passwordSalt);

            await _dbContext.Accounts.AddAsync(account);
            await _dbContext.SaveChangesAsync();

            return account;
        }

        public async Task<Account> AuthenticateAccountAsync(EmailAddress email, string password)
        {
            var account = await _dbContext.Accounts
                .FirstOrDefaultAsync(a => a.Email == email)
                ?? throw new InvalidCredentialsException();

            if (!account.Verified)
            {
                throw new AccountNotVerifiedException(account);
            }

            if (!_passwordManager.VerifyPasswordHash(
                password, account.PasswordHash, account.PasswordSalt))
            {
                throw new InvalidCredentialsException();
            }

            return account;
        }

        public async Task<Account?> GetByIdAsync(AccountId id)
        {
            return await _dbContext.Accounts
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task UpdateAccount(Account account)
        {
            _dbContext.Accounts.Update(account);
            await _dbContext.SaveChangesAsync();
        }

        private Task<bool> EmailAlreadyExists(EmailAddress email)
        {
            return _dbContext.Accounts.AnyAsync(a => a.Email == email);
        }

        public async Task DeleteAsync(AccountId id)
        {
            var accountToDelete = await _dbContext
                .Accounts
                .FirstOrDefaultAsync(a => a.Id == id)
                ?? throw new AccountDoesNotExistException(id);

            _dbContext.Remove(accountToDelete);
            await _dbContext.SaveChangesAsync();
        }
    }
}
