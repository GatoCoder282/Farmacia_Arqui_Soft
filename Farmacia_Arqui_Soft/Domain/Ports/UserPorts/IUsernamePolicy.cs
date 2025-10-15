using System.Collections.Generic;

namespace Farmacia_Arqui_Soft.Domain.Ports.UserPorts
{
    public interface IUsernamePolicy
    {
        string BuildBase(string first, string? second, string last, string ci);
        string Normalize(string input);
        string EnsureUnique(string baseUsername, IEnumerable<string> existing);
    }
}
