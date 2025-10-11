using Farmacia_Arqui_Soft.Domain.Ports;
using Farmacia_Arqui_Soft.Domain.Models;

namespace Farmacia_Arqui_Soft.Infraestructure.Persistence
{
    public abstract class RepositoryFactory
    {
        public  abstract IRepository<T> CreateRepository<T>() where T : class;
        
    }
}
