using Farmacia_Arqui_Soft.Models;
using Farmacia_Arqui_Soft.Repositories;
using Farmacia_Arqui_Soft.Interfaces;

namespace Farmacia_Arqui_Soft.Factory
{
    public abstract class RepositoryFactory
    {
        public  abstract IRepository<T> CreateRepository<T>() where T : class;
        
    }
}
