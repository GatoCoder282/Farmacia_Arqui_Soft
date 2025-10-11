using Farmacia_Arqui_Soft.Domain.Models;
using Farmacia_Arqui_Soft.Domain.Ports;

namespace Farmacia_Arqui_Soft.Infraestructure.Persistence
{
    public sealed class UserRepositoryFactory : RepositoryFactory
    {
        public override IRepository<T> CreateRepository<T>() where T : class
        {
            if (typeof(T) == typeof(User))
                return (IRepository<T>)new UserRepository();

            throw new NotImplementedException(
                $"No existe implementación CRUD para {typeof(T).Name}"
            );
        }
    }
}
