namespace TexasTaco.Shared.Exceptions
{
    public class InvalidRequestParametersException(string message)
        : Exception(message);
}
