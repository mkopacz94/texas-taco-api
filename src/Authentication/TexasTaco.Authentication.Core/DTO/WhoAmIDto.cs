namespace TexasTaco.Authentication.Core.DTO
{
    public record WhoAmIDto(
        string? Email,
        string? AccountId,
        string? Role);
}
