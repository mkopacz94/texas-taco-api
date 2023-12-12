using TexasTaco.Authentication.Core.Models;

namespace TexasTaco.Authentication.Core.Services.Verification
{
    public interface IEmailVerificationService
    {
        Task EnqueueVerificationEmail(Account account);
    }
}
