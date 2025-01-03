﻿using TexasTaco.Authentication.Core.Entities;
using TexasTaco.Shared.Authentication;
using TexasTaco.Shared.ValueObjects;

namespace TexasTaco.Authentication.Core.Repositories
{
    public interface IAuthenticationRepository
    {
        Task<Account> CreateAccountAsync(EmailAddress email, Role role, string password);
        Task<Account> AuthenticateAccountAsync(EmailAddress email, string password);
        Task<Account?> GetByIdAsync(AccountId id);
        Task UpdateAccount(Account account);
        Task DeleteAsync(AccountId id);
    }
}
