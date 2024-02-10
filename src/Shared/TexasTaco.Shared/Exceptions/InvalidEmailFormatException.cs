namespace TexasTaco.Shared.Exceptions
{
    public class InvalidEmailFormatException(string message)
        : TexasTacoException(message);
}
