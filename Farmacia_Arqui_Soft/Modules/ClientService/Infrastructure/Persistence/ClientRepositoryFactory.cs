using Farmacia_Arqui_Soft.Domain.Ports;
using Farmacia_Arqui_Soft.Infraestructure.Persistence;

using ClientEntity = Farmacia_Arqui_Soft.Modules.ClientService.Domain.Client;

namespace Farmacia_Arqui_Soft.Modules.ClientService.Infrastructure.Persistence
{
    public sealed class ClientRepositoryFactory : RepositoryFactory
    {
        public override IRepository<T> CreateRepository<T>() where T : class
        {
            if (typeof(T) == typeof(ClientEntity))
                return (IRepository<T>)new ClientRepository();

            throw new NotImplementedException($"No existe implementación CRUD para {typeof(T).Name}");
        }
    }
}
