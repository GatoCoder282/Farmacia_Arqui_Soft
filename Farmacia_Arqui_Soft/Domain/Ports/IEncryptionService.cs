using System.Threading.Tasks;

namespace Farmacia_Arqui_Soft.Domain.Ports
{
    public interface IEncryptionService
    {
        /// <summary>
        /// Encripta un entero (ID) a una cadena de texto segura para URLs.
        /// </summary>
        /// <param name="id">El ID entero a encriptar (ej. el ID de un Cliente).</param>
        /// <returns>La cadena encriptada.</returns>
        string EncryptId(int id);

        /// <summary>
        /// Desencripta una cadena de texto a un ID entero.
        /// </summary>
        /// <param name="encryptedId">La cadena encriptada proveniente de la URL.</param>
        /// <returns>El ID entero desencriptado.</returns>
        /// <exception cref="FormatException">Lanzada si la cadena no es un formato válido.</exception>
        int DecryptId(string encryptedId);
    }
}
