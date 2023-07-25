using System.Security.Cryptography;

namespace FileOrbisApi.PassGenerate
{
    public class PasswordGenerate
    {
        public static string GenerateStrongPassword(int length = 12)
        {
            const string validChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()-_=+[]{}|;:,.<>?";
            using (var rng = new RNGCryptoServiceProvider())
            {
                var randomBytes = new byte[length];
                rng.GetBytes(randomBytes);

                var password = new char[length];
                for (int i = 0; i < length; i++)
                {
                    password[i] = validChars[randomBytes[i] % validChars.Length];
                }

                return new string(password);
            }
        }
    }
}
