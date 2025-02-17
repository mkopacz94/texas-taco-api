using TexasTaco.Authentication.Core.ValueObjects;
using TexasTaco.Shared.ValueObjects;

namespace TexasTaco.Authentication.Core.DTO
{
    public record SignInResultDto(
        SessionId SessionId,
        AccountId AccountId,
        string Role);
}
