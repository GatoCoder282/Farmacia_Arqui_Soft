using System.Security.Cryptography;
using Farmacia_Arqui_Soft.Domain.Ports.UserPorts;

namespace Farmacia_Arqui_Soft.Infraestructure.Security
{
    public sealed class CryptoPasswordGenerator : IPasswordGenerator
    {
        public string Generate(int length)
        {
            if (length < 8) length = 8;

            const string lowers = "abcdefghijklmnopqrstuvwxyz";
            const string uppers = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string digits = "0123456789";
            const string symbols = "!@#$%^&*_-";

            var required = new[]
            {
                lowers[RandomNumberGenerator.GetInt32(lowers.Length)],
                uppers[RandomNumberGenerator.GetInt32(uppers.Length)],
                digits[RandomNumberGenerator.GetInt32(digits.Length)],
                symbols[RandomNumberGenerator.GetInt32(symbols.Length)]
            }.ToList();

            var all = lowers + uppers + digits + symbols;
            while (required.Count < length)
                required.Add(all[RandomNumberGenerator.GetInt32(all.Length)]);

            for (int i = required.Count - 1; i > 0; i--)
            {
                int j = RandomNumberGenerator.GetInt32(i + 1);
                (required[i], required[j]) = (required[j], required[i]);
            }
            return new string(required.ToArray());
        }
    }
}
