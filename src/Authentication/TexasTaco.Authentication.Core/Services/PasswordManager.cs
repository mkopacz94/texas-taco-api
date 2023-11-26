using System.Text;

namespace TexasTaco.Authentication.Core.Services
{
    internal class PasswordManager : IPasswordManager
    {
        public void HashPassword(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new System.Security.Cryptography.HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }
    }
}
