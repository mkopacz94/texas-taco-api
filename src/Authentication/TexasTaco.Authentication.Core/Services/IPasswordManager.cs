namespace TexasTaco.Authentication.Core.Services
{
    public interface IPasswordManager
    {
        void HashPassword(string password, out byte[] passwordHash, out byte[] passwordSalt);
    }
}
