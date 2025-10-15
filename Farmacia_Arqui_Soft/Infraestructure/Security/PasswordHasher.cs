using Farmacia_Arqui_Soft.Domain.Ports.UserPorts;
using System.Security.Cryptography;
using System.Text;

namespace Farmacia_Arqui_Soft.Infraestructure.Security
{
    public class PasswordHasher : IPasswordHasher
    {
         
        private const int Pbkdf2Iterations = 100_000;
        private const int SaltSize = 16;
        private const int KeySize = 32;

        public string Hash(string password)
        {
            var salt = RandomNumberGenerator.GetBytes(SaltSize);
#if NET6_0_OR_GREATER
            var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Pbkdf2Iterations, HashAlgorithmName.SHA256, KeySize);
#else
            using var derive = new Rfc2898DeriveBytes(password, salt, Pbkdf2Iterations);
            var hash = derive.GetBytes(KeySize);
#endif
            return $"PBKDF2|{Pbkdf2Iterations}|{Convert.ToBase64String(salt)}|{Convert.ToBase64String(hash)}";
        }

        public bool Verify(string password, string stored)
        {
            var parts = stored.Split('|');
            if (parts.Length != 4 || parts[0] != "PBKDF2") return false;
            var iterations = int.Parse(parts[1]);
            var salt = Convert.FromBase64String(parts[2]);
            var expected = Convert.FromBase64String(parts[3]);
#if NET6_0_OR_GREATER
            var actual = Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations, HashAlgorithmName.SHA256, expected.Length);
#else
            using var derive = new Rfc2898DeriveBytes(password, salt, iterations);
            var actual = derive.GetBytes(expected.Length);
#endif
            if (actual.Length != expected.Length) return false;
            var diff = 0;
            for (int i = 0; i < actual.Length; i++) diff |= actual[i] ^ expected[i];
            return diff == 0;
        }
    }
}
