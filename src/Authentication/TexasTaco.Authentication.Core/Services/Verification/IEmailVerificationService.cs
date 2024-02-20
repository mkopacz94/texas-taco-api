using TexasTaco.Authentication.Core.Entities;

namespace TexasTaco.Authentication.Core.Services.Verification
{
    public interface IEmailVerificationService
    {
        Task EnqueueVerificationEmail(Account account);
    }
}
