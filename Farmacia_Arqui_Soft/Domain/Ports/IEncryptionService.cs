using System.Threading.Tasks;

namespace Farmacia_Arqui_Soft.Domain.Ports
{
    public interface IEncryptionService
    {
        
        
        string EncryptId(int id);
        int DecryptId(string encryptedId);
    }
}
