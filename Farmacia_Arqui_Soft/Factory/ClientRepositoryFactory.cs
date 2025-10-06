using Farmacia_Arqui_Soft.Interfaces;
using Farmacia_Arqui_Soft.Models;
using Farmacia_Arqui_Soft.Repositories;

namespace Farmacia_Arqui_Soft.Factory
{
    public sealed class ClientRepositoryFactory : RepositoryFactory
    {
        public override IRepository<T> CreateRepository<T>() where T : class
        {
            if (typeof(T) == typeof(Client))
                return (IRepository<T>)new ClientRepository();

            throw new NotImplementedException($"No existe implementación CRUD para {typeof(T).Name}");
        }
    }
}
