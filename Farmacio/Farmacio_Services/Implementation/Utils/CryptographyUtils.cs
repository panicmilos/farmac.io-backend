using System;
using System.Security.Cryptography;
using System.Text;

namespace Farmacio_Services.Implementation.Utils
{
    public class CryptographyUtils
    {
        public static string GetRandomSalt()
        {
            var randomGenerator = RandomNumberGenerator.Create();
            var saltBytes = new byte[32];
            randomGenerator.GetBytes(saltBytes);

            return Convert.ToBase64String(saltBytes);
        }

        public static string GetSaltedAndHashedPassword(string password, string salt)
        {
            var sha265Algorithm = SHA256.Create();
            var saltedPassword = password + salt;
            var saltedPasswordBytes = Encoding.Unicode.GetBytes(saltedPassword);
            var hashedSaltedPasswordBytes = sha265Algorithm.ComputeHash(saltedPasswordBytes);

            return Convert.ToBase64String(hashedSaltedPasswordBytes);
        }
    }
}