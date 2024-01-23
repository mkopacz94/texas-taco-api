using TexasTaco.Authentication.Core.ValueObjects;

namespace TexasTaco.Authentication.Core.DTO
{
    public record SignInResultDto(SessionId SessionId, AccountId AccountId);
}
