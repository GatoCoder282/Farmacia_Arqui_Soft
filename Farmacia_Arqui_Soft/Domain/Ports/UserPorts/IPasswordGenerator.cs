namespace Farmacia_Arqui_Soft.Domain.Ports.UserPorts
{
    public interface IPasswordGenerator
    {
        string Generate(int length);
    }
}

