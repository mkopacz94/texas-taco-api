namespace TexasTaco.Authentication.Core.DTO
{
    public record UserSignInDto(
        string Email,
        string Password,
        string? RequiredRole);
}
