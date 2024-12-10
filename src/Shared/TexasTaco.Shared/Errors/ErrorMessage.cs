namespace TexasTaco.Shared.Errors
{
    public record ErrorMessage(
        string ErrorCode,
        string Message,
        string? AdditionalInformation = null);
}
