namespace Farmacia_Arqui_Soft.Domain.Ports
{
    public interface IPasswordGenerator
    {
        string Generate(int length);
    }
}

