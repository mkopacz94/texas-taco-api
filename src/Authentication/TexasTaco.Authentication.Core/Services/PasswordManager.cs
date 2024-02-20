using System.Security.Cryptography;
using System.Text;

namespace TexasTaco.Authentication.Core.Services
{
    internal class PasswordManager : IPasswordManager
    {
        public void HashPassword(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }

        public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512(passwordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if(passwordHash.Length < i + 1)
                {
                    return false;
                }

                if (computedHash[i] != passwordHash[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}
