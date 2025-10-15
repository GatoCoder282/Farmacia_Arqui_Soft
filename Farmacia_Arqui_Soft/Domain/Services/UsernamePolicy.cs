using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using Farmacia_Arqui_Soft.Domain.Ports;

namespace Farmacia_Arqui_Soft.Domain.Services
{
    public sealed class UsernamePolicy : IUsernamePolicy
    {
        public string BuildBase(string first, string? second, string last, string ci)
        {
            static string Initial(string? s) => string.IsNullOrWhiteSpace(s) ? "" : s.Trim()[0].ToString();

            string digits3 = new string(ci.Where(char.IsDigit).ToArray());
            digits3 = digits3.Length >= 3 ? digits3[..3] : digits3;

            string core = Normalize(Initial(first) + Initial(second) + last);

            int maxTotal = 20;
            int maxCoreLen = Math.Max(1, maxTotal - digits3.Length);
            if (core.Length > maxCoreLen) core = core[..maxCoreLen];

            return core + digits3;
        }

        public string Normalize(string s)
        {
            s = s.Trim().ToLowerInvariant();

            var normalized = s.Normalize(NormalizationForm.FormD);
            var builder = new StringBuilder();
            foreach (var ch in normalized)
            {
                var uc = CharUnicodeInfo.GetUnicodeCategory(ch);
                if (uc != UnicodeCategory.NonSpacingMark) builder.Append(ch);
            }
            s = builder.ToString().Normalize(NormalizationForm.FormC);

            return Regex.Replace(s, "[^a-z0-9]", "");
        }

        public string EnsureUnique(string baseUsername, IEnumerable<string> existing)
        {
            var set = new HashSet<string>(existing, StringComparer.OrdinalIgnoreCase);
            var candidate = baseUsername;
            var i = 1;
            while (set.Contains(candidate))
            {
                var suffix = i.ToString();
                var head = baseUsername;
                if (head.Length + suffix.Length > 20)
                    head = head[..Math.Max(1, 20 - suffix.Length)];
                candidate = head + suffix;
                i++;
            }
            return candidate;
        }
    }
}
