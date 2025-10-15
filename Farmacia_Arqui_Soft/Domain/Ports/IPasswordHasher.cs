namespace Farmacia_Arqui_Soft.Domain.Ports
{
    public interface IPasswordHasher
    {
        string Hash(string plain);
        bool Verify(string plain, string hashed);
    }
}
