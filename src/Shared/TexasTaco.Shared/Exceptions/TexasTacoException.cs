namespace TexasTaco.Shared.Exceptions
{
    public abstract class TexasTacoException :Exception
    {
        protected TexasTacoException(string? message) : base(message)
        {
        }
    }
}
