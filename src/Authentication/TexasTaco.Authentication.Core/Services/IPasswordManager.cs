namespace TexasTaco.Authentication.Core.Abstractions
{
    public interface IPasswordManager
    {
        void HashPassword(string password, out byte[] passwordHash, out byte[] passwordSalt);
        bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);
    }
}
