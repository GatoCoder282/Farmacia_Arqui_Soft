using Farmacia_Arqui_Soft.Models;
using Farmacia_Arqui_Soft.Repositories;

namespace Farmacia_Arqui_Soft.Repository
{
    public sealed class ProviderRepositoryFactory : RepositoryFactory
    {
        public override IRepository<T> CreateRepository<T>() where T : class
        {
            if (typeof(T) == typeof(Provider))
                return (IRepository<T>)new ProviderRepository();

            throw new NotImplementedException(
                $"No existe implementación CRUD para {typeof(T).Name}"
            );
        }
    }
}
