using System.Collections.Generic;

namespace Farmacia_Arqui_Soft.Domain.Ports
{
    public interface IUsernamePolicy
    {
        // Construye el username base a partir de nombres y CI (sin garantizar unicidad)
        string BuildBase(string first, string? second, string last, string ci);
        // Normaliza a [a-z0-9], sin tildes
        string Normalize(string input);
        // Asegura unicidad con tope de 20 chars, dado un set existente
        string EnsureUnique(string baseUsername, IEnumerable<string> existing);
    }
}
