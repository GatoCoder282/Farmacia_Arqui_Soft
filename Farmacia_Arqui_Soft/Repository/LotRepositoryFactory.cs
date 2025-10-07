using Farmacia_Arqui_Soft.Models;
using Farmacia_Arqui_Soft.Repositories;

namespace Farmacia_Arqui_Soft.Repository
{
    public class LotRepositoryFactory : RepositoryFactory
    {
        public override IRepository<T> CreateRepository<T>() where T : class
        {
            if (typeof(T) == typeof(Lot))
                return (IRepository<T>)new LotRepository();

            throw new NotImplementedException($"No existe implementaci�n CRUD para {typeof(T).Name}");
        }
    }
}
