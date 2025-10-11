using Farmacia_Arqui_Soft.Domain.Models;
using Farmacia_Arqui_Soft.Repositories;
using Farmacia_Arqui_Soft.Repository;

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
