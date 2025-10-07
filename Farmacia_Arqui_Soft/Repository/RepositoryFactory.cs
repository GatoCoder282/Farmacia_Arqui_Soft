using Farmacia_Arqui_Soft.Models;
using Farmacia_Arqui_Soft.Repositories;

namespace Farmacia_Arqui_Soft.Repository
{
    public abstract class RepositoryFactory
    {
        public  abstract IRepository<T> CreateRepository<T>() where T : class;
        
    }
}
