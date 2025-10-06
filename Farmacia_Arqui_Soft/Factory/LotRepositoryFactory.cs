using Farmacia_Arqui_Soft.Interfaces;
using Farmacia_Arqui_Soft.Models;
using Farmacia_Arqui_Soft.Data;

namespace Farmacia_Arqui_Soft.Factory
{
    public class LotRepositoryFactory : RepositoryFactory
    {
        public override IRepository<T> CreateRepository<T>()
        {
            if (typeof(T) == typeof(Lot))
                return (IRepository<T>)new LotData();

            throw new NotImplementedException("Repository not found for this type");
        }
    }
}
